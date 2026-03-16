using System;
using System.IO;

namespace BeatyBit.PathBuddy;

/// <summary>
/// Represents a relative file system path.
/// </summary>
public class RelativePath : PathBase
{
  private RelativePath(string path) : base(path) { }

  /// <summary>
  /// Parses a string into a <see cref="RelativePath"/>.
  /// </summary>
  /// <param name="path">The string to parse.</param>
  /// <returns>A new <see cref="RelativePath"/> instance.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the path is null.</exception>
  /// <exception cref="ArgumentException">Thrown if the path is absolute.</exception>
  public static RelativePath Parse(string path)
  {
    ThrowIfInvalid(path);
    return new RelativePath(path);
  }

  internal static string ThrowIfInvalid(string path)
    => path is null            ? throw new ArgumentNullException(nameof(path)) :
       Path.IsPathRooted(path) ? throw new ArgumentException($"The file system path {path} is absolute path.")
                                 : path;

  /// <summary>
  /// Combines two relative paths.
  /// </summary>
  public static RelativePath operator /(RelativePath left, RelativePath right) => new RelativePath(Path.Combine(left.FullPath, right.FullPath));
  /// <summary>
  /// Combines a relative path with a string segment.
  /// </summary>
  public static RelativePath operator /(RelativePath left, string       right) => new RelativePath(Path.Combine(left.FullPath, ThrowIfInvalid(right)));

  /// <summary>Gets the file name and extension of the path string.</summary>
  public string FileName                 => Path.GetFileName(FullPath);
  /// <summary>Gets the file name of the file path without the extension.</summary>
  public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FullPath);
  /// <summary>Gets the extension of the path string.</summary>
  public string Extension                => Path.GetExtension(FullPath);
  /// <summary>Determines whether a path includes a file name extension.</summary>
  public bool   HasExtension()           => Path.HasExtension(FullPath);

  /// <summary>Changes the extension of a path string.</summary>
  public RelativePath ChangeExtension(string? extension) => new RelativePath(Path.ChangeExtension(FullPath, extension));

  /// <summary>Returns the directory information for the specified path.</summary>
  public RelativePath? GetDirectory()
  {
    var dir = Path.GetDirectoryName(FullPath);
    if(dir is null) return null;

    // Ensure remains relative
    ThrowIfInvalid(dir);
    return new RelativePath(dir);
  }

  // Equality
  /// <summary>Determines whether the specified object is equal to the current object.</summary>
  public override bool Equals(object? obj) => obj is RelativePath rp && string.Equals(FullPath, rp.FullPath, OsDependentComparisonType);

  /// <summary>Serves as the default hash function.</summary>
  public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(FullPath);

  /// <summary>Determines whether two specified <see cref="RelativePath"/> objects have the same value.</summary>
  public static bool operator ==(RelativePath? left, RelativePath? right)
    => ReferenceEquals(left, right) || (left is not null && right is not null && left.Equals(right));

  /// <summary>Determines whether two specified <see cref="RelativePath"/> objects have different values.</summary>
  public static bool operator !=(RelativePath? left, RelativePath? right) => !(left == right);
}