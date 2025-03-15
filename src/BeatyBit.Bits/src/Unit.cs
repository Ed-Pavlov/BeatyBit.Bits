namespace BeatyBit.Bits;

/// <summary>
/// Is used as generic parameter to specify "no generic parameter"
/// </summary>
internal struct Unit
{
  private static Unit? _default; // create on demand

  public Unit Default => _default ??= new Unit();
}