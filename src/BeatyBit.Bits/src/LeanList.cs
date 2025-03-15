using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BeatyBit.Bits;


/// <summary>
/// A memory-efficient list implementation that stores first four elements inline and falls back to <see cref="List{T}"/> for additional elements.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class LeanList4<T> : IList<T>
{
  private T?       _0;
  private T?       _1;
  private T?       _2;
  private T?       _3;
  private List<T>? _list;

  public LeanList4(int capacity = 1)
  {
    if(capacity > 4)
      _list = new List<T>(capacity - 4);
  }

  public LeanList4(ICollection<T> collection) => AddRange(collection);

  public int Count { get; private set; }

  /// <inheritdoc cref="IList{T}.Add"/>
  public void Add(T item)
  {
    if(Count >= 4)
      List.Add(item);
    else
      SetItem(Count)(item);

    Count += 1;
  }

  /// <inheritdoc cref="List{T}.AddRange"/>
  public void AddRange(ICollection<T> items)
  {
    var toSkip = 0;
    if(Count < 4)
      foreach(var item in items)
      {
        if(Count > 4)
          break;

        Add(item);
        toSkip++;
      }

    List.AddRange(items.Skip(toSkip));
  }

  /// <inheritdoc cref="IList{T}.this"/>
  public T this[int index]
  {
    get
    {
      if(index < 0 || Count < index) throw new ArgumentOutOfRangeException(nameof(index));
      return GetItem(index);
    }

    set
    {
      if(index < 0 || Count < index) throw new ArgumentOutOfRangeException(nameof(index));
      SetItem(index)(value);
    }
  }

  /// <inheritdoc cref="IList{T}.IndexOf(T)"/>
  public int IndexOf(T item)
  {
    for(var i = 0; i < Count; i++)
      if(Equals(item, GetItem(i)))
        return i;

    return -1;
  }

  /// <inheritdoc cref="IList{T}.Contains"/>
  public bool Contains(T item) => IndexOf(item) >= 0;

  /// <inheritdoc cref="IList{T}.Clear"/>
  public void Clear()
  {
    _0 = _1 = _2 = _3 = default;
    _list?.Clear();
    Count = 0;
  }

  /// <inheritdoc cref="IList{T}.GetEnumerator"/>
  public IEnumerator<T> GetEnumerator()
  {
    for(var i = 0; i < Count; i++)
      yield return GetItem(i);
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public bool IsReadOnly                          => false;

  /// <summary> NotSupported </summary>
  public void CopyTo(T[]   array, int arrayIndex) => throw new NotSupportedException();
  /// <summary> NotSupported </summary>
  public bool Remove(T     item)          => throw new NotSupportedException();
  /// <summary> NotSupported </summary>
  public void Insert(int   index, T item) => throw new NotSupportedException();
  /// <summary> NotSupported </summary>
  public void RemoveAt(int index) => throw new NotSupportedException();

  private List<T> List => _list ??= new List<T>();

  private Action<T> SetItem(int index)
    => index switch
    {
      0 => value => _0 = value,
      1 => value => _1 = value,
      2 => value => _2 = value,
      3 => value => _3 = value,
      _ => value => _list![index - 4] = value
    };

  private T GetItem(int index)
    => index switch
    {
      0 => _0!,
      1 => _1!,
      2 => _2!,
      3 => _3!,
      _ => _list![index - 4]
    };
}