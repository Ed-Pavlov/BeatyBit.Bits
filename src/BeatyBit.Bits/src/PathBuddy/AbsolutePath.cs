using System;
using System.IO;
using BeatyBit.Lifetimes;
using JetBrains.Annotations;

namespace BeatyBit.PathBuddy;

/// <summary>
/// Represents an absolute file system path.
/// </summary>
[PublicAPI]
public class AbsolutePath : PathBase
{
  /// <summary>
  /// Represents an empty absolute path.
  /// </summary>
  public static AbsolutePath Empty = new AbsolutePath("");

  private AbsolutePath(string path) : base(path) { }

  /// <summary>
  /// Parses a string into an <see cref="AbsolutePath"/>.
  /// </summary>
  /// <param name="path">The string to parse.</param>
  /// <returns>A new <see cref="AbsolutePath"/> instance.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the path is null.</exception>
  /// <exception cref="ArgumentException">Thrown if the path is not absolute.</exception>
  public static AbsolutePath Parse(string path)
    => path is null              ? throw new ArgumentNullException(nameof(path)) :
       ! Path.IsPathRooted(path) ? throw new ArgumentException($"The file system path {path} is not an absolute path.") : new AbsolutePath(path);

  /// <summary>Combines an absolute path with a relative path.</summary>
  public static AbsolutePath operator /(AbsolutePath left, RelativePath right) => new(Path.Combine(left.FullPath, right.FullPath));

  /// <summary>Combines an absolute path with a relative path string segment.</summary>
  public static AbsolutePath operator /(AbsolutePath left, string right) => new(Path.Combine(left.FullPath, RelativePath.ThrowIfInvalid(right)));

  /// <summary>Determines whether the given path refers to an existing directory on disk.</summary>
  public bool DirectoryExist() => Directory.Exists(FullPath);

  /// <summary>Determines whether the specified file exists.</summary>
  public bool FileExist() => File.Exists(FullPath);

  /// <summary>Gets a <see cref="DirectoryInfo"/> for the path if it exists.</summary>
  public DirectoryInfo GetDirectoryInfo()
    => DirectoryExist() ? new DirectoryInfo(FullPath) : throw new InvalidOperationException($"Directory {FullPath} does not exist.");

  /// <summary>Gets the file name and extension of the path string.</summary>
  public string FileName => Path.GetFileName(FullPath);

  /// <summary>Gets the file name of the file path without the extension.</summary>
  public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FullPath);

  /// <summary>Gets the extension of the path string.</summary>
  public string Extension => Path.GetExtension(FullPath);

  /// <summary>Determines whether a path includes a file name extension.</summary>
  public bool HasExtension() => Path.HasExtension(FullPath);

  /// <summary>Changes the extension of a path string.</summary>
  public AbsolutePath ChangeExtension(string? extension) => new(Path.ChangeExtension(FullPath, extension));

  /// <summary>Adds an extension to a path string.</summary>
  public AbsolutePath AddExtension(string? extension) => new(FullPath + "." + extension);

  /// <summary>Returns the parent directory for the specified path.</summary>
  public AbsolutePath GetDirectory()
  {
    var dir = Path.GetDirectoryName(FullPath);
    if(dir is null) throw new InvalidOperationException($"Parent directory for {FullPath} does not exist.");

    return Path.IsPathRooted(dir) ? new AbsolutePath(dir) : throw new InvalidOperationException($"Unexpected non-rooted directory derived from absolute path: {dir}");
  }

  // Equality
  /// <summary>Determines whether the specified object is equal to the current object.</summary>
  public override bool Equals(object? obj) => obj is AbsolutePath ap && string.Equals(FullPath, ap.FullPath, StringComparison.OrdinalIgnoreCase);

  /// <summary>Serves as the default hash function.</summary>
  public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(FullPath);

  /// <summary>Determines whether two specified <see cref="AbsolutePath"/> objects have the same value.</summary>
  public static bool operator ==(AbsolutePath? left, AbsolutePath? right)
    => ReferenceEquals(left, right) || ( left is not null && right is not null && left.Equals(right) );

  /// <summary>Determines whether two specified <see cref="AbsolutePath"/> objects have different values.</summary>
  public static bool operator !=(AbsolutePath? left, AbsolutePath? right) => ! ( left == right );

  /// <summary>Creates a zero-byte file at the specified path.</summary>
  public void CreateFile() => File.Create(FullPath).Dispose();

  /// <inheritdoc cref="Directory.CreateDirectory"/>
  public void CreateDirectory() => Directory.CreateDirectory(FullPath);

  /// <inheritdoc cref="File.Delete"/>
  public  void DeleteFile()      => File.Delete(FullPath);

  /// <inheritdoc cref="Directory.Delete(string, bool)"/>
  public  void DeleteDirectory(bool recursive = true) => Directory.Delete(FullPath, recursive);

  /// <inheritdoc cref="Path.GetTempPath"/>
  public static AbsolutePath GetTempDirectory() => Parse(Path.GetTempPath());

  /// <summary>Creates a temporary file with an optional filename that is automatically deleted when the lifetime terminates.</summary>
  /// <param name="lifetime">The lifetime that controls the temporary file's existence.</param>
  /// <param name="filename">Optional custom filename. If null, a unique temporary filename is generated.</param>
  /// <returns>An <see cref="AbsolutePath"/> representing the created temporary file.</returns>
  public static AbsolutePath CreateTempFile(Lifetime lifetime, string? filename = null)
  {
    var filePath = filename is null ? Parse(Path.GetTempFileName()) : GetTempDirectory() / filename;
    lifetime.Bracket(
      () => filePath.CreateFile(),
      () => filePath.DeleteFile()
    );

    return filePath;
  }

  /// <summary>Creates a temporary directory with the specified subdirectory name.</summary>
  public static AbsolutePath CreateTempDirectory(Lifetime lifetime, string? subDirectory = null)
  {
    var directory = Parse(Path.GetTempPath()) / (subDirectory ?? Path.GetRandomFileName());

    lifetime.Bracket(
      () => directory.CreateDirectory(),
      () => directory.DeleteDirectory()
    );

    return directory;
  }
}