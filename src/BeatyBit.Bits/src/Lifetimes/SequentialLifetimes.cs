using System;
using System.Threading;

namespace BeatyBit.Lifetimes;

/// <summary>
/// Manages a sequence of lifetimes, ensuring that the previous lifetime is terminated before the next one begins.
/// </summary>
public class SequentialLifetimes
{
  private readonly Lifetime           _parentLifetime;
  private          LifetimeDefinition _currentDef = LifetimeDefinition.Terminated;

  /// <summary>
  /// Initializes a new instance of the <see cref="SequentialLifetimes"/> class.
  /// </summary>
  /// <param name="lifetime">The parent lifetime that governs the entire sequence.</param>
  public SequentialLifetimes(Lifetime lifetime) => _parentLifetime = lifetime;

  /// <summary>
  /// Terminates the current lifetime and returns a new one.
  /// </summary>
  /// <returns>The next <see cref="Lifetime"/> in the sequence.</returns>
  public Lifetime Next()
  {
    TerminateCurrent();
    var next = new LifetimeDefinition(_parentLifetime);
    return TrySetNewAndTerminateOld(next).Lifetime;
  }

  /// <summary>
  /// Terminates the current lifetime in the sequence.
  /// </summary>
  public void TerminateCurrent() => TrySetNewAndTerminateOld(LifetimeDefinition.Terminated);

  private LifetimeDefinition TrySetNewAndTerminateOld(LifetimeDefinition newLifetimeDefinition)
  {
    var old = Interlocked.Exchange(ref _currentDef, newLifetimeDefinition);

    try
    {
      old?.Terminate();
    }
    catch(Exception)
    { /* Log if necessary */
    }

    return newLifetimeDefinition;
  }
}