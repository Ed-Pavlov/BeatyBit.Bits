namespace BeatyBit.Lifetimes;

/// <summary>
/// Provides a mechanism for handling the termination of a lifetime.
/// </summary>
public interface ITerminationHandler
{
  /// <summary>
  /// Called when the associated lifetime is terminated.
  /// </summary>
  /// <param name="lifetime">The lifetime that has been terminated.</param>
  void OnTermination(Lifetime lifetime);
}