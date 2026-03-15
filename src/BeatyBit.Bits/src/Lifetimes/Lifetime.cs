using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeatyBit.Lifetimes;

/// <summary>
/// A high-performance, lightweight, and composable alternative to <see cref="CancellationToken"/>, representing a bounded lifespan for an operation or object.
/// </summary>
public readonly struct Lifetime : IEquatable<Lifetime>
{
  /// <summary>
  /// Gets a <see cref="Lifetime"/> that is never terminated.
  /// </summary>
  public static readonly Lifetime Eternal = LifetimeDefinition.Eternal.Lifetime;

  /// <summary>
  /// Gets a <see cref="Lifetime"/> that is already terminated.
  /// </summary>
  public static readonly Lifetime Terminated = LifetimeDefinition.Terminated.Lifetime;

  internal readonly LifetimeDefinition Definition;

  internal Lifetime(LifetimeDefinition definition) => Definition = definition ?? throw new ArgumentNullException(nameof(definition));

  /// <summary>
  /// Gets the current status of the lifetime.
  /// </summary>
  public LifetimeStatus Status => Definition.Status;

  /// <summary>
  /// Gets a value indicating whether the lifetime is currently active.
  /// </summary>
  public bool IsAlive => Status == LifetimeStatus.Alive;

  /// <summary>
  /// Gets a value indicating whether the lifetime is not currently active.
  /// </summary>
  public bool IsNotAlive => ! IsAlive;

  /// <summary>
  /// Gets a value indicating whether this lifetime is eternal and will never be terminated.
  /// </summary>
  public bool IsEternal => Definition.IsEternal;

  /// <summary>
  /// Schedules an action to be executed when the lifetime is terminated.
  /// </summary>
  /// <param name="action">The action to execute.</param>
  /// <returns>The current <see cref="Lifetime"/> instance.</returns>
  public Lifetime OnTermination(Action action)
  {
    Definition.OnTermination(action);
    return this;
  }

  /// <summary>
  /// Schedules a disposable object to be disposed when the lifetime is terminated.
  /// </summary>
  /// <param name="disposable">The object to dispose.</param>
  /// <returns>The current <see cref="Lifetime"/> instance.</returns>
  public Lifetime OnTermination(IDisposable disposable)
  {
    Definition.OnTermination(disposable);
    return this;
  }

  /// <summary>
  /// Schedules a disposable object to be disposed when the lifetime is terminated.
  /// </summary>
  /// <param name="disposables">The object to dispose.</param>
  /// <returns>The current <see cref="Lifetime"/> instance.</returns>
  public Lifetime OnTermination(params IDisposable[] disposables)
  {
    foreach(var disposable in disposables)
      OnTermination(disposable);

    return this;
  }

  /// <summary>
  /// Schedules a termination handler to be called when the lifetime is terminated.
  /// </summary>
  /// <param name="handler">The handler to call.</param>
  /// <returns>The current <see cref="Lifetime"/> instance.</returns>
  public Lifetime OnTermination(ITerminationHandler handler)
  {
    Definition.OnTermination(handler);
    return this;
  }

  /// <summary>
  /// Tries to schedule an action to be executed when the lifetime is terminated.
  /// </summary>
  /// <param name="action">The action to execute.</param>
  /// <returns><c>true</c> if the action was scheduled; <c>false</c> if the lifetime is already terminating or terminated.</returns>
  public bool TryOnTermination(Action action) => Definition.TryAdd(action);

  /// <summary>
  /// Tries to schedule a disposable object to be disposed when the lifetime is terminated.
  /// </summary>
  /// <param name="disposable">The object to dispose.</param>
  /// <returns><c>true</c> if the object was scheduled for disposal; <c>false</c> if the lifetime is already terminating or terminated.</returns>
  public bool TryOnTermination(IDisposable disposable) => Definition.TryAdd(disposable);

  /// <summary>
  /// Creates a new nested <see cref="LifetimeDefinition"/> that is terminated when this lifetime is terminated.
  /// </summary>
  /// <returns>A new <see cref="LifetimeDefinition"/>.</returns>
  public LifetimeDefinition CreateNested() => new LifetimeDefinition(this);

  /// <summary>
  /// Throws an <see cref="OperationCanceledException"/> if the lifetime is not alive.
  /// </summary>
  public void ThrowIfNotAlive()
  {
    if(IsNotAlive) throw new OperationCanceledException("Lifetime has been canceled or terminated.");
  }

  /// <summary>
  /// Creates a <see cref="CancellationToken"/> that is canceled when the lifetime is terminated.
  /// </summary>
  /// <returns>A <see cref="CancellationToken"/>.</returns>
  public CancellationToken ToCancellationToken() => Definition.ToCancellationToken();

  /// <summary>
  /// Implicitly converts a <see cref="Lifetime"/> to a <see cref="CancellationToken"/>.
  /// </summary>
  /// <param name="lifetime">The lifetime to convert.</param>
  public static implicit operator CancellationToken(Lifetime lifetime) => lifetime.ToCancellationToken();

  /// <summary>
  /// Brackets an operation within the lifetime. The opening function is executed, and the closing action is scheduled for termination.
  /// </summary>
  /// <typeparam name="T">The type of the resource.</typeparam>
  /// <param name="opening">The function to create the resource.</param>
  /// <param name="closing">The action to clean up the resource.</param>
  /// <returns>The created resource.</returns>
  public T Bracket<T>(Func<T> opening, Action<T> closing)
  {
    if(opening == null) throw new ArgumentNullException(nameof(opening));
    if(closing == null) throw new ArgumentNullException(nameof(closing));

    using var cookie = Definition.UsingExecuteIfAlive();
    if(! cookie.Succeed) throw new OperationCanceledException();

    var res = opening();
    if(! Definition.TryAdd(() => closing(res))) closing(res);
    return res;
  }

  /// <summary>
  /// Brackets an action within the lifetime. The opening action is executed, and the closing action is scheduled for termination.
  /// </summary>
  /// <param name="opening">The opening action.</param>
  /// <param name="closing">The closing action.</param>
  public void Bracket(Action opening, Action closing)
  {
    if(opening == null) throw new ArgumentNullException(nameof(opening));
    if(closing == null) throw new ArgumentNullException(nameof(closing));

    using var cookie = Definition.UsingExecuteIfAlive();
    if(! cookie.Succeed) throw new OperationCanceledException();

    opening();
    if(! Definition.TryAdd(closing)) closing();
  }

  /// <summary>
  /// Creates a new <see cref="Lifetime"/> and executes an action within its scope. The lifetime is terminated when the action completes.
  /// </summary>
  /// <param name="action">The action to execute.</param>
  public static void Using(Action<Lifetime> action)
  {
    using var def = new LifetimeDefinition();
    action(def.Lifetime);
  }

  /// <summary>
  /// Creates a new <see cref="Lifetime"/> and executes an asynchronous action within its scope. The lifetime is terminated when the task completes.
  /// </summary>
  /// <param name="action">The asynchronous action to execute.</param>
  public static async Task UsingAsync(Func<Lifetime, Task> action)
  {
    using var def = new LifetimeDefinition();
    await action(def.Lifetime).ConfigureAwait(false);
  }

  /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
  public bool Equals(Lifetime other) => ReferenceEquals(Definition, other.Definition);

  /// <summary>Indicates whether this instance and a specified object are equal.</summary>
  public override bool Equals(object? obj) => obj is Lifetime other && Equals(other);

  /// <summary>Returns the hash code for this instance.</summary>
  public override int GetHashCode() => Definition.GetHashCode();

  /// <summary>Indicates whether two <see cref="Lifetime"/> objects are equal.</summary>
  public static bool operator ==(Lifetime left, Lifetime right) => left.Equals(right);

  /// <summary>Indicates whether two <see cref="Lifetime"/> objects are not equal.</summary>
  public static bool operator !=(Lifetime left, Lifetime right) => ! left.Equals(right);

  /// <summary>
  /// Creates a disposable wrapper around a <see cref="LifetimeDefinition"/> that automatically terminates the lifetime when disposed.
  /// </summary>
  /// <returns></returns>
  public static DisposableLifetime CreateDisposable() => new DisposableLifetime();
}