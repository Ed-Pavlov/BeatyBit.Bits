namespace BeatyBit.Lifetimes;

/// <summary>
/// Describes the status of a <see cref="Lifetime"/>. A lifetime is created as <see cref="Alive"/> and eventually becomes <see cref="Terminated"/>.
/// </summary>
public enum LifetimeStatus
{
  /// <summary>The lifetime is active and has not been terminated.</summary>
  Alive = 0,
  /// <summary>The lifetime is in the process of being canceled, but termination actions have not yet started.</summary>
  Canceling = 1,
  /// <summary>The lifetime is executing its termination actions.</summary>
  Terminating = 2,
  /// <summary>The lifetime has completed termination and is no longer active.</summary>
  Terminated = 3
}