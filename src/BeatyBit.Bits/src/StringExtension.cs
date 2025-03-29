namespace BeatyBit.Bits;

/// <summary>
/// Contains extension methods for working with strings.
/// </summary>
public static class StringExtension
{
  /// <summary>
  /// Determines whether the specified string is null, empty, or consists only of a single space character.
  /// </summary>
  /// <param name="value">The string to evaluate.</param>
  /// <returns><c>true</c> if the string is null, empty, or a single space; otherwise, <c>false</c>.</returns>
  public static bool IsNothing(this string? value) => string.IsNullOrWhiteSpace(value);
}