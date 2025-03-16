using System;
using System.Collections.Generic;

namespace BeatyBit.Bits;

/// <summary>
/// Provides extension methods for collections to simplify common operations.
/// </summary>
public static class CollectionExtension
{
  /// <summary>
  /// Safely retrieves a value from a dictionary, returning a default value if the key is not found.
  /// </summary>
  /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
  /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
  /// <param name="dictionary">The dictionary to search in.</param>
  /// <param name="key">The key to look up.</param>
  /// <param name="defaultValue">The default value to return if the key is not found.</param>
  /// <returns>The value associated with the key if found; otherwise, the default value.</returns>
  /// <exception cref="ArgumentNullException">Thrown when dictionary or key is null.</exception>
  public static TValue? GetValueSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default)
  {
    if(dictionary is null) throw new ArgumentNullException(nameof(dictionary));
    if(key is null) throw new ArgumentNullException(nameof(key));

    return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
  }

  /// <summary>
  /// Gets a value from the dictionary or creates and adds it if it doesn't exist.
  /// </summary>
  /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
  /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
  /// <param name="dictionary">The dictionary to search in or add to.</param>
  /// <param name="key">The key to look up or add.</param>
  /// <param name="createValue">The function to create a new value if the key is not found.</param>
  /// <returns>The existing or newly created value associated with the key.</returns>
  /// <exception cref="ArgumentNullException">Thrown when dictionary, key, or createValue is null.</exception>
  public static TValue GetOrCreateValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createValue)
  {
    if(dictionary is null) throw new ArgumentNullException(nameof(dictionary));
    if(key is null) throw new ArgumentNullException(nameof(key));
    if(createValue is null) throw new ArgumentNullException(nameof(createValue));

    if(! dictionary.TryGetValue(key, out var value))
    {
      value = createValue();
      dictionary.Add(key, value);
    }

    return value;
  }
}