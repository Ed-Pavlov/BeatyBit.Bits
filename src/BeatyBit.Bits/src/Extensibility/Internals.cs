using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BeatyBit.Bits.Extensibility;

/// <summary>
/// Provides extension methods for accessing internal fields or properties in interfaces implementing the <see cref="IInternal{T}"/> series of interfaces.
/// </summary>
/// <remarks>
/// These utility methods allow users to retrieve the internal state of objects while retaining type safety.
/// </remarks>
public static class ExtensibilityExtension
{
  /// <summary>
  /// Retrieves the internal fields or properties represented by a single generic type parameter.
  /// </summary>
  /// <typeparam name="T">The type of the internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T> GetInternals<T>(this IInternal<T> obj) => obj;

  /// <summary>
  /// Retrieves the internal fields or properties represented by two generic type parameters.
  /// </summary>
  /// <typeparam name="T1">The type of the first internal field or property.</typeparam>
  /// <typeparam name="T2">The type of the second internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T1, T2}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2> GetInternals<T1, T2>(this IInternal<T1, T2> obj) => obj;

  /// <summary>
  /// Retrieves the internal fields or properties represented by three generic type parameters.
  /// </summary>
  /// <typeparam name="T1">The type of the first internal field or property.</typeparam>
  /// <typeparam name="T2">The type of the second internal field or property.</typeparam>
  /// <typeparam name="T3">The type of the third internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T1, T2, T3}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3> GetInternals<T1, T2, T3>(this IInternal<T1, T2, T3> obj) => obj;

  /// <summary>
  /// Retrieves the internal fields or properties represented by four generic type parameters.
  /// </summary>
  /// <typeparam name="T1">The type of the first internal field or property.</typeparam>
  /// <typeparam name="T2">The type of the second internal field or property.</typeparam>
  /// <typeparam name="T3">The type of the third internal field or property.</typeparam>
  /// <typeparam name="T4">The type of the fourth internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T1, T2, T3, T4}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4> GetInternals<T1, T2, T3, T4>(this IInternal<T1, T2, T3, T4> obj) => obj;

  /// <summary>
  /// Retrieves the internal fields or properties represented by five generic type parameters.
  /// </summary>
  /// <typeparam name="T1">The type of the first internal field or property.</typeparam>
  /// <typeparam name="T2">The type of the second internal field or property.</typeparam>
  /// <typeparam name="T3">The type of the third internal field or property.</typeparam>
  /// <typeparam name="T4">The type of the fourth internal field or property.</typeparam>
  /// <typeparam name="T5">The type of the fifth internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T1, T2, T3, T4, T5}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4, T5> GetInternals<T1, T2, T3, T4, T5>(this IInternal<T1, T2, T3, T4, T5> obj) => obj;

  /// <summary>
  /// Retrieves the internal fields or properties represented by six generic type parameters.
  /// </summary>
  /// <typeparam name="T1">The type of the first internal field or property.</typeparam>
  /// <typeparam name="T2">The type of the second internal field or property.</typeparam>
  /// <typeparam name="T3">The type of the third internal field or property.</typeparam>
  /// <typeparam name="T4">The type of the fourth internal field or property.</typeparam>
  /// <typeparam name="T5">The type of the fifth internal field or property.</typeparam>
  /// <typeparam name="T6">The type of the sixth internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T1, T2, T3, T4, T5, T6}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4, T5, T6> GetInternals<T1, T2, T3, T4, T5, T6>(this IInternal<T1, T2, T3, T4, T5, T6> obj) => obj;

  /// <summary>
  /// Retrieves the internal fields or properties represented by seven generic type parameters.
  /// </summary>
  /// <typeparam name="T1">The type of the first internal field or property.</typeparam>
  /// <typeparam name="T2">The type of the second internal field or property.</typeparam>
  /// <typeparam name="T3">The type of the third internal field or property.</typeparam>
  /// <typeparam name="T4">The type of the fourth internal field or property.</typeparam>
  /// <typeparam name="T5">The type of the fifth internal field or property.</typeparam>
  /// <typeparam name="T6">The type of the sixth internal field or property.</typeparam>
  /// <typeparam name="T7">The type of the seventh internal field or property.</typeparam>
  /// <param name="obj">The object implementing the <see cref="IInternal{T1, T2, T3, T4, T5, T6, T7}"/> interface.</param>
  /// <returns>The input object for further chaining or access to its internals.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4, T5, T6, T7> GetInternals<T1, T2, T3, T4, T5, T6, T7>(this IInternal<T1, T2, T3, T4, T5, T6, T7> obj) => obj;


  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T> GetInternals<T>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2> GetInternals<T1, T2>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T1, T2> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T1, T2>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3> GetInternals<T1, T2, T3>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T1, T2, T3> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T1, T2, T3>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4> GetInternals<T1, T2, T3, T4>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T1, T2, T3, T4> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T1, T2, T3, T4>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4, T5> GetInternals<T1, T2, T3, T4, T5>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T1, T2, T3, T4, T5> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T1, T2, T3, T4, T5>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4, T5, T6> GetInternals<T1, T2, T3, T4, T5, T6>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T1, T2, T3, T4, T5, T6> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T1, T2, T3, T4, T5, T6>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IInternal<T1, T2, T3, T4, T5, T6, T7> GetInternals<T1, T2, T3, T4, T5, T6, T7>(this object obj)
    => obj is null
         ? throw new ArgumentNullException(nameof(obj))
         : obj as IInternal<T1, T2, T3, T4, T5, T6, T7> ?? throw new ArgumentException($"Object of type {obj.GetType()} does not implement {typeof(IInternal<T1, T2, T3, T4, T5, T6, T7>)}");

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T>(this object obj, [NotNullWhen(true)] out IInternal<T>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T>;
    return internals is not null;
  }

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T1, T2>(this object obj, out IInternal<T1, T2>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T1, T2>;
    return internals is not null;
  }

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T1, T2, T3>(this object obj, out IInternal<T1, T2, T3>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T1, T2, T3>;
    return internals is not null;
  }

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T1, T2, T3, T4>(this object obj, out IInternal<T1, T2, T3, T4>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T1, T2, T3, T4>;
    return internals is not null;
  }

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T1, T2, T3, T4, T5>(this object obj, out IInternal<T1, T2, T3, T4, T5>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T1, T2, T3, T4, T5>;
    return internals is not null;
  }

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T1, T2, T3, T4, T5, T6>(this object obj, out IInternal<T1, T2, T3, T4, T5, T6>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T1, T2, T3, T4, T5, T6>;
    return internals is not null;
  }

  /// <inheritdoc cref="GetInternals{T}(IInternal{T})"/>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool GetInternalsSafe<T1, T2, T3, T4, T5, T6, T7>(this object obj, out IInternal<T1, T2, T3, T4, T5, T6, T7>? internals)
  {
    if(obj is null) throw new ArgumentNullException(nameof(obj));

    internals = obj as IInternal<T1, T2, T3, T4, T5, T6, T7>;
    return internals is not null;
  }
}

/// <summary>
/// This interface provides access to the internal fields in the generic form for the sake of possible extensibility by the end user.
/// See implementation for details.
/// </summary>
public interface IInternal<out T1>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T1 Member1 { get; }
}

///<inheritdoc />
public interface IInternal<out T1, out T2> : IInternal<T1>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T2 Member2 { get; }
}

///<inheritdoc />
public interface IInternal<out T1, out T2, out T3> : IInternal<T1, T2>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T3 Member3 { get; }
}

///<inheritdoc />
public interface IInternal<out T1, out T2, out T3, out T4> : IInternal<T1, T2, T3>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T4 Member4 { get; }
}

///<inheritdoc />
public interface IInternal<out T1, out T2, out T3, out T4, out T5> : IInternal<T1, T2, T3, T4>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T5 Member5 { get; }
}

///<inheritdoc />
public interface IInternal<out T1, out T2, out T3, out T4, out T5, out T6> : IInternal<T1, T2, T3, T4, T5>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T6 Member6 { get; }
}

///<inheritdoc />
public interface IInternal<out T1, out T2, out T3, out T4, out T5, out T6, out T7> : IInternal<T1, T2, T3, T4, T5, T6>
{
  /// <summary>
  /// Gets the internal member or property represented by this generic parameter.
  /// </summary>
  T7 Member7 { get; }
}