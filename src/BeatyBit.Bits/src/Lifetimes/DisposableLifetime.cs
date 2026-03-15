using System;

namespace BeatyBit.Lifetimes;

/// <summary>
/// Represents a disposable wrapper around a <see cref="LifetimeDefinition"/> that automatically terminates the lifetime when disposed.
/// </summary>
public class DisposableLifetime : IDisposable
{
  private readonly LifetimeDefinition _definition;

  /// <summary>
  /// Initializes a new instance of the <see cref="DisposableLifetime"/> class.
  /// </summary>
  /// <param name="definition">The lifetime definition to wrap. If null, a new lifetime definition is created.</param>
  public DisposableLifetime(LifetimeDefinition? definition = null) => _definition = definition ?? new LifetimeDefinition();

  /// <summary>
  /// Implicitly converts a <see cref="DisposableLifetime"/> to a <see cref="Lifetime"/>.
  /// </summary>
  /// <param name="disposableLifetime">The disposable lifetime to convert.</param>
  /// <returns>The underlying lifetime.</returns>
  /// <exception cref="ArgumentNullException">Thrown when disposableLifetime is null.</exception>
  public static implicit operator Lifetime(DisposableLifetime disposableLifetime)
    => disposableLifetime == null ? throw new ArgumentNullException(nameof(disposableLifetime)) : disposableLifetime._definition.Lifetime;

  /// <summary>
  /// Disposes the underlying lifetime definition, terminating the lifetime.
  /// </summary>
  public void Dispose() => _definition.Dispose();
}