using System.Collections;
using System.Collections.Generic;

namespace BeatyBit.Bits;

/// <summary>
/// Provides empty enumerator to avoid creating multiple instances.
/// </summary>
public static class Empty
{
  private static readonly IEnumerator? _enumerator;

  /// <summary>
  /// A shared instance of an empty enumerator to be reused wherever needed.
  /// </summary>
  public static readonly IEnumerator Enumerator = _enumerator ??= new EmptyEnumerator();

  private class EmptyEnumerator : IEnumerator
  {
    public bool MoveNext() => false;
    public void Reset() { }
    public object? Current => null;
  }
}

/// <summary>
/// Provides the empty generic list to avoid creating multiple instances.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public static class Empty<T>
{
  private static readonly List<T>? _list;

  /// <summary>
  /// A shared instance of an empty generic list to be reused wherever needed.
  /// </summary>
  public static readonly List<T> List = _list ??= [];
}
