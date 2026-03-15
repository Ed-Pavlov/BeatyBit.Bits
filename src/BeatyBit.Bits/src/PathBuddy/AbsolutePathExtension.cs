using System;
using System.IO;
using BeatyBit.Lifetimes;

namespace BeatyBit.PathBuddy;

/// <summary>
/// Extension methods for <see cref="AbsolutePath"/>.
/// </summary>
public static class AbsolutePathExtension
{
  /// <summary>Associates the path with a lifetime, automatically deleting the file or directory when the lifetime terminates.</summary>
  /// <param name="path">The path to manage.</param>
  /// <param name="lifetime">The lifetime that controls the path's existence.</param>
  /// <returns>The same <see cref="AbsolutePath"/> instance for chaining.</returns>
  public static AbsolutePath SetLifetime(this AbsolutePath path, Lifetime lifetime)
  {
    lifetime.OnTermination(() =>
      {
        if(path.FileExist())
          path.DeleteFile();
        else if(path.DirectoryExist())
          path.DeleteDirectory();
      }
    );

    return path;
  }
}