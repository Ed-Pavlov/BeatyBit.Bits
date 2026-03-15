using System;

namespace BeatyBit.PathBuddy;

/// <summary>
/// Base class for file system paths.
/// </summary>
public abstract class PathBase : IEquatable<PathBase>
{

  /// <summary>
  /// The underlying string representation of the path.
  /// </summary>
  public string FullPath { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="PathBase"/> class.
  /// </summary>
  /// <param name="path">The path string.</param>
  protected PathBase(string path) => FullPath = path;

  /// <summary>
  /// Returns the string representation of the path.
  /// </summary>
  /// <returns>The path string.</returns>
  public override string ToString() => FullPath;

  /// <inheritdoc/>
  public bool Equals(PathBase? other) => other is not null && ( ReferenceEquals(this, other) || FullPath  == other.FullPath  );

  /// <inheritdoc/>
  public override bool Equals(object? obj) => Equals(obj as PathBase);

  /// <inheritdoc/>
  public override int GetHashCode() => FullPath  .GetHashCode();
}