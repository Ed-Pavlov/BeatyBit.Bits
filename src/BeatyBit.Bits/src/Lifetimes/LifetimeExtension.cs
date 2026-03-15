using System;

namespace BeatyBit.Lifetimes;

/// <summary>
/// Provides extension methods for <see cref="Lifetime"/>.
/// </summary>
public static class LifetimeExtension
{
  /// <summary>
  /// Attaches a disposable object to a lifetime, ensuring it is disposed when the lifetime terminates.
  /// </summary>
  /// <typeparam name="T">The type of the disposable object.</typeparam>
  /// <param name="disposable">The disposable object.</param>
  /// <param name="lifetime">The lifetime to attach to.</param>
  /// <returns>The disposable object.</returns>
  public static T SetLifetime<T>(this T disposable, Lifetime lifetime) where T : IDisposable
  {
    lifetime.OnTermination(disposable);
    return disposable;
  }
}