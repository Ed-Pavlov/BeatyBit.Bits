using BeatyBit.Lifetimes;
using BeatyBit.PathBuddy;
using FluentAssertions;

namespace BeatyBit.Bits.Test;

[TestFixture]
public class PathBuddyTest
{
  [Test]
  public void Parse_should_be_exception_when_path_is_not_absolute()
  {
    // --arrange
    var path = "not/absolute/path";

    // --act
    Action act = () => AbsolutePath.Parse(path);

    // --assert
    act.Should().Throw<ArgumentException>();
  }

  [Test]
  public void Parse_should_be_success_when_path_is_absolute()
  {
    // --arrange
    var path = Path.GetFullPath("absolute/path");

    // --act
    var absolutePath = AbsolutePath.Parse(path);

    // --assert
    absolutePath.FullPath.Should().Be(path);
  }

  [Test]
  public void Combine_with_relative_path_should_be_success()
  {
    // --arrange
    var absolutePath = AbsolutePath.Parse(Path.GetFullPath("absolute/path"));
    var relativePath = RelativePath.Parse("relative/path");

    // --act
    var result = absolutePath / relativePath;

    // --assert
    result.FullPath.Should().Be(Path.Combine(absolutePath.FullPath, relativePath.FullPath));
  }

  [Test]
  public void Combine_with_string_should_be_success()
  {
    // --arrange
    var absolutePath = AbsolutePath.Parse(Path.GetFullPath("absolute/path"));
    var relativePath = "relative/path";

    // --act
    var result = absolutePath / relativePath;

    // --assert
    result.FullPath.Should().Be(Path.Combine(absolutePath.FullPath, relativePath));
  }

  [Test]
  public void GetDirectoryInfo_should_be_exception_when_directory_not_exist()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("not/existing/path"));

    // --act
    Action act = () => path.GetDirectoryInfo();

    // --assert
    act.Should().Throw<InvalidOperationException>();
  }

  [Test]
  public void GetDirectoryInfo_should_be_success_when_directory_exist()
  {
    // --arrange
    using var lifetime = Lifetime.CreateDisposable();

    var path = AbsolutePath.CreateTempDirectory(lifetime);

    // --act
    var directoryInfo = path.GetDirectoryInfo();

    // --assert
    directoryInfo.Should().NotBeNull();
    directoryInfo.FullName.Should().Be(path.FullPath);
  }

  [Test]
  public void FileName_should_be_correct()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var fileName = path.FileName;

    // --assert
    fileName.Should().Be("file.txt");
  }

  [Test]
  public void FileNameWithoutExtension_should_be_correct()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var fileName = path.FileNameWithoutExtension;

    // --assert
    fileName.Should().Be("file");
  }

  [Test]
  public void Extension_should_be_correct()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var extension = path.Extension;

    // --assert
    extension.Should().Be(".txt");
  }

  [Test]
  public void HasExtension_should_be_true_when_extension_exists()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var hasExtension = path.HasExtension();

    // --assert
    hasExtension.Should().BeTrue();
  }

  [Test]
  public void HasExtension_should_be_false_when_extension_not_exists()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file"));

    // --act
    var hasExtension = path.HasExtension();

    // --assert
    hasExtension.Should().BeFalse();
  }

  [Test]
  public void ChangeExtension_should_be_correct()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var newPath = path.ChangeExtension(".md");

    // --assert
    newPath.FullPath.Should().Be(Path.ChangeExtension(path.FullPath, ".md"));
  }

  [Test]
  public void AddExtension_should_be_correct()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file"));

    // --act
    var newPath = path.AddExtension("txt");

    // --assert
    newPath.FullPath.Should().Be(path.FullPath + ".txt");
  }

  [Test]
  public void GetDirectory_should_be_correct()
  {
    // --arrange
    var path     = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    var expected = Path.GetDirectoryName(path.FullPath);

    // --act
    var directory = path.GetDirectory();

    // --assert
    directory.FullPath.Should().Be(expected);
  }

  [Test]
  public void RelativePath_Parse_should_be_exception_when_path_is_absolute()
  {
    // --arrange
    var path = Path.GetFullPath("absolute/path");

    // --act
    Action act = () => RelativePath.Parse(path);

    // --assert
    act.Should().Throw<ArgumentException>();
  }

  [Test]
  public void RelativePath_Parse_should_be_success_when_path_is_relative()
  {
    // --arrange
    var path = "relative/path";

    // --act
    var relativePath = RelativePath.Parse(path);

    // --assert
    relativePath.FullPath.Should().Be(path);
  }

  [Test]
  public void RelativePath_Combine_with_relative_path_should_be_success()
  {
    // --arrange
    var path1 = RelativePath.Parse("relative/path1");
    var path2 = RelativePath.Parse("relative/path2");

    // --act
    var result = path1 / path2;

    // --assert
    result.FullPath.Should().Be(Path.Combine(path1.FullPath, path2.FullPath));
  }

  [Test]
  public void RelativePath_Combine_with_string_should_be_success()
  {
    // --arrange
    var path1 = RelativePath.Parse("relative/path1");
    var path2 = "relative/path2";

    // --act
    var result = path1 / path2;

    // --assert
    result.FullPath.Should().Be(Path.Combine(path1.FullPath, path2));
  }

  [Test]
  public void CreateTempFile_should_create_file()
  {
    AbsolutePath tempFile;

    // --arrange
    using(var lifetime = Lifetime.CreateDisposable())
    {
      // --act
      tempFile = AbsolutePath.CreateTempFile(lifetime);

      // --assert
      tempFile.FileExist().Should().BeTrue();
    }

    // --assert
    tempFile.FileExist().Should().BeFalse();
  }

  [Test]
  public void CreateDirectory_should_create_directory()
  {
    // --arrange
    AbsolutePath tempDirectory;

    using(var lifetime = Lifetime.CreateDisposable())
    {
      // --act
      tempDirectory = AbsolutePath.CreateTempDirectory(lifetime);

      // --assert
      tempDirectory.DirectoryExist().Should().BeTrue();
    }

    // --assert
    tempDirectory.DirectoryExist().Should().BeFalse();
  }

  [Test]
  public void RelativePath_GetDirectory_should_be_correct()
  {
    // --arrange
    var path     = RelativePath.Parse("some/path/file.txt");
    var expected = Path.GetDirectoryName(path.FullPath);

    // --act
    var directory = path.GetDirectory();

    // --assert
    directory?.FullPath.Should().Be(expected);
  }
}