using JetBrains.Annotations;

namespace BeatyBit.Bits;

/// <summary>
/// Is used as generic parameter to specify "no generic parameter"
/// </summary>
public struct Unit
{
  /// <summary>
  /// The default (and only) value of <see cref="Unit"/>
  /// </summary>
  [PublicAPI]
  public static readonly Unit Default;
}