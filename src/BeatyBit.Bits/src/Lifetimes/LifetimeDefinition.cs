using System;
using System.Threading;
using BeatyBit.Bits;

namespace BeatyBit.Lifetimes;

/// <summary>
/// Represents the definition and controller for a <see cref="Lifetime"/>, analogous to <see cref="CancellationTokenSource"/>.
/// This class is responsible for managing the state and termination of a lifetime.
/// </summary>
public class LifetimeDefinition : IDisposable
{
  /// <summary>
  /// Gets a <see cref="LifetimeDefinition"/> that is never terminated.
  /// </summary>
  public static readonly LifetimeDefinition Eternal;
  /// <summary>
  /// Gets a <see cref="LifetimeDefinition"/> that is already terminated.
  /// </summary>
  public static readonly LifetimeDefinition Terminated;

  static LifetimeDefinition()
  {
    Eternal    = new LifetimeDefinition(isEternal: true);
    Terminated = new LifetimeDefinition(isTerminated: true);
  }

  private volatile int                      _status;
  private volatile int                      _executingCount;
  private          CancellationTokenSource? _cts;

  private readonly object             _resourceLock = new object();
  private          LeanList4<object>? _resources;
  private readonly bool               _isEternal;

  /// <summary>
  /// Initializes a new instance of the <see cref="LifetimeDefinition"/> class.
  /// </summary>
  public LifetimeDefinition()
  {
    _status = (int) LifetimeStatus.Alive;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="LifetimeDefinition"/> class with a parent lifetime.
  /// The new lifetime will be terminated when the parent lifetime is terminated.
  /// </summary>
  /// <param name="parent">The parent lifetime.</param>
  public LifetimeDefinition(Lifetime parent) : this()
  {
    parent.Definition.Attach(this);
  }

  private LifetimeDefinition(bool isEternal = false, bool isTerminated = false)
  {
    _isEternal = isEternal;
    _status    = isTerminated ? (int) LifetimeStatus.Terminated : (int) LifetimeStatus.Alive;
    if(isTerminated) Terminate();
  }

  /// <summary>
  /// Gets the <see cref="Lifetime"/> associated with this definition.
  /// </summary>
  public Lifetime Lifetime => new Lifetime(this);

  /// <summary>
  /// Gets the current status of the lifetime.
  /// </summary>
  public LifetimeStatus Status => (LifetimeStatus) _status;

  /// <summary>
  /// Gets a value indicating whether this lifetime is eternal and will never be terminated.
  /// </summary>
  public bool IsEternal => _isEternal;

  /// <summary>
  /// Gets the number of currently executing blocks that require the lifetime to be alive.
  /// </summary>
  public int ExecutingCount => _executingCount;

  /// <summary>
  /// Disposes the <see cref="LifetimeDefinition"/>, initiating the termination of the associated lifetime.
  /// </summary>
  public void Dispose() => Terminate();

  /// <summary>
  /// Initiates the termination of the lifetime.
  /// This will transition the status and execute all registered termination actions.
  /// </summary>
  public void Terminate()
  {
    if(_isEternal || Status > LifetimeStatus.Canceling)
      return;

    // Transition from Alive -> Canceling
    if(Interlocked.CompareExchange(ref _status, (int) LifetimeStatus.Canceling, (int) LifetimeStatus.Alive) == (int) LifetimeStatus.Alive)
    {
      _cts?.Cancel();

      // Spin until all ExecuteIfAlive blocks have finished
      SpinWait.SpinUntil(() => _executingCount == 0);

      // Transition from Canceling -> Terminating
      Interlocked.CompareExchange(ref _status, (int) LifetimeStatus.Terminating, (int) LifetimeStatus.Canceling);

      Destruct();
    }
  }

  private void Destruct()
  {
    LeanList4<object>? resources;

    lock(_resourceLock)
    {
      resources  = _resources;
      _resources = null;
    }

    if(resources != null)
    {
      // Execute in LIFO order
      for(var i = resources.Count - 1; i >= 0; i--)
      {
        try
        {
          switch(resources[i])
          {
            case Action a:               a(); break;
            case LifetimeDefinition ld:  ld.Terminate(); break;
            case IDisposable d:          d.Dispose(); break;
            case ITerminationHandler th: th.OnTermination(Lifetime); break;
          }
        }
        catch(Exception)
        {
          // In a full implementation, log the error.
          // We swallow it here to ensure subsequent resources still terminate.
        }
      }
    }

    // Transition to Terminated
    _status = (int) LifetimeStatus.Terminated;
  }

  internal bool TryAdd(object action)
  {
    if(action == null) throw new ArgumentNullException(nameof(action));
    if(_isEternal) return true;

    lock(_resourceLock)
    {
      if(Status >= LifetimeStatus.Terminating)
        return false;

      _resources ??= new LeanList4<object>();
      _resources.Add(action);
      return true;
    }
  }

  internal void OnTermination(object action)
  {
    if(TryAdd(action)) return;

    // If it failed to add, it means it's terminating/terminated. Execute synchronously.
    try
    {
      switch(action)
      {
        case Action a:               a(); break;
        case IDisposable d:          d.Dispose(); break;
        case ITerminationHandler th: th.OnTermination(Lifetime); break;
      }
    }
    catch(Exception)
    { /* Log */
    }

    throw new InvalidOperationException("Cannot add termination action if lifetime is terminating or terminated.");
  }

  internal void Attach(LifetimeDefinition child)
  {
    if(child == null) throw new ArgumentNullException(nameof(child));
    if(child.IsEternal) return;

    if(!TryAdd(child))
    {
      child.Terminate();
    }
  }

  /// <summary>
  /// Creates a <see cref="CancellationToken"/> that is canceled when the lifetime is terminated.
  /// </summary>
  /// <returns>A <see cref="CancellationToken"/>.</returns>
  public CancellationToken ToCancellationToken()
  {
    if(_cts == null)
    {
      lock(_resourceLock)
      {
        if(Status >= LifetimeStatus.Terminating)
          return new CancellationToken(true);

        _cts ??= new CancellationTokenSource();
      }
    }

    return _cts.Token;
  }

  /// <summary>
  /// Enters a block that requires the lifetime to be alive.
  /// This is used to prevent termination while the block is executing.
  /// </summary>
  /// <returns>A disposable cookie that must be disposed to exit the block.</returns>
  public ExecuteIfAliveCookie UsingExecuteIfAlive()
  {
    return new ExecuteIfAliveCookie(this);
  }

  /// <summary>
  /// A disposable struct that represents an execution block that requires the lifetime to be alive.
  /// </summary>
  public struct ExecuteIfAliveCookie : IDisposable
  {
    private readonly LifetimeDefinition _def;
    /// <summary>
    /// Gets a value indicating whether the execution block was successfully entered.
    /// </summary>
    public readonly bool Succeed;

    internal ExecuteIfAliveCookie(LifetimeDefinition def)
    {
      _def = def;

      while(true)
      {
        var s = _def._status;

        if(s != (int) LifetimeStatus.Alive)
        {
          Succeed = false;
          return;
        }

        Interlocked.Increment(ref _def._executingCount);

        if(_def._status == (int) LifetimeStatus.Alive)
        {
          Succeed = true;
          return;
        }

        // Rolled back if state changed right after incrementing
        Interlocked.Decrement(ref _def._executingCount);
      }
    }

    /// <summary>
    /// Disposes the cookie, signaling the end of the execution block.
    /// </summary>
    public void Dispose()
    {
      if(Succeed)
      {
        Interlocked.Decrement(ref _def._executingCount);
      }
    }
  }
}