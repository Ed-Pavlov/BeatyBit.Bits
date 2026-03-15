using System;

namespace BeatyBit.Bits;

/// <summary>
/// Represents a Maybe monad that encapsulates an optional value.
/// </summary>
/// <typeparam name="T">The type of the value that may or may not be present.</typeparam>
public readonly struct Maybe<T>
{
  /// <summary>
  /// Represents an empty Maybe instance with no value.
  /// </summary>
  public static readonly Maybe<T> Nothing = new Maybe<T>();

  /// <summary>
  /// Initializes a new instance of the <see cref="Maybe{T}"/> struct with the specified value.
  /// </summary>
  /// <param name="value">The value to wrap in the Maybe instance.</param>
  public Maybe(T value)
  {
    Value    = value;
    HasValue = true;
  }

  /// <summary>
  /// Gets a value indicating whether this Maybe instance has a value.
  /// </summary>
  public bool HasValue { get; }

  /// <summary>
  /// Gets the value contained in this Maybe instance.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when attempting to access the Value of an empty Maybe instance.</exception>
  public T Value => HasValue ? field : throw new InvalidOperationException("Has no value");
}

/// <summary>
/// Provides extension methods for working with Maybe instances.
/// </summary>
public static class Maybe
{
  /// <summary>
  /// Converts a value to a Maybe instance.
  /// </summary>
  /// <typeparam name="T">The type of the value to convert.</typeparam>
  /// <param name="value">The value to convert to a Maybe instance.</param>
  /// <returns>A new Maybe instance containing the specified value.</returns>
  public static Maybe<T> ToMaybe<T>(this T value) => new Maybe<T>(value);

  /// <summary>
  /// Safely gets the value from a Maybe instance, returning null if the instance has no value.
  /// </summary>
  /// <typeparam name="T">The type of the value in the Maybe instance.</typeparam>
  /// <param name="maybe">The Maybe instance to get the value from.</param>
  /// <returns>The value if present; otherwise, null.</returns>
  public static T? GetValueSafe<T>(this Maybe<T> maybe) => maybe.HasValue ? maybe.Value : default;
}