using System;
using JetBrains.Annotations;

namespace BeatyBit.Bits;

/// <summary>
/// A set of miscellaneous extensions that don't fit into a specific category.
/// </summary>
public static class MiscExtension
{
  /// <summary>
  /// Invokes the specified action on the given object and returns the object.
  /// </summary>
  /// <typeparam name="T">The type of the object.</typeparam>
  /// <param name="value">The object on which to perform the action.</param>
  /// <param name="action">The action to perform on the object.</param>
  /// <returns>The original object, after the action has been applied.</returns>
  /// <remarks>
  /// This  is useful for configuring objects inline or calling several methods in a row without the need for temporary variables.
  /// </remarks>
  public static T Apply<T>(this T value, [InstantHandle] Action<T> action)
  {
    if(value is null) throw new ArgumentNullException(nameof(value));
    if(action is null) throw new ArgumentNullException(nameof(action));

    action(value);
    return value;
  }
}