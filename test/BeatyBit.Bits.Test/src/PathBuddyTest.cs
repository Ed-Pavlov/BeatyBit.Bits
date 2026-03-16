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

  [Test]
  public void RelativePath_FileName_should_be_correct()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");

    // --act
    var fileName = path.FileName;

    // --assert
    fileName.Should().Be("file.txt");
  }

  [Test]
  public void RelativePath_FileNameWithoutExtension_should_be_correct()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");

    // --act
    var fileName = path.FileNameWithoutExtension;

    // --assert
    fileName.Should().Be("file");
  }

  [Test]
  public void RelativePath_Extension_should_be_correct()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");

    // --act
    var extension = path.Extension;

    // --assert
    extension.Should().Be(".txt");
  }

  [Test]
  public void RelativePath_HasExtension_should_be_true_when_extension_exists()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");

    // --act
    var hasExtension = path.HasExtension();

    // --assert
    hasExtension.Should().BeTrue();
  }

  [Test]
  public void RelativePath_HasExtension_should_be_false_when_extension_not_exists()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file");

    // --act
    var hasExtension = path.HasExtension();

    // --assert
    hasExtension.Should().BeFalse();
  }

  [Test]
  public void RelativePath_ChangeExtension_should_be_correct()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");

    // --act
    var newPath = path.ChangeExtension(".md");

    // --assert
    newPath.FullPath.Should().Be(Path.ChangeExtension(path.FullPath, ".md"));
  }

  [Test]
  public void RelativePath_Equals_should_be_true_for_same_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void RelativePath_Equals_should_be_false_for_different_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file1.txt");
    var path2 = RelativePath.Parse("some/path/file2.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  [Platform("Win")]
  public void RelativePath_Equals_should_be_case_insensitive()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/FILE.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void RelativePath_Equals_should_be_case_sensitive()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/FILE.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_GetHashCode_should_be_same_for_equal_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var hash1 = path1.GetHashCode();
    var hash2 = path2.GetHashCode();

    // --assert
    hash1.Should().Be(hash2);
  }

  [Test]
  [Platform("Win")]
  public void RelativePath_GetHashCode_should_be_case_insensitive()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/FILE.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var hash1 = path1.GetHashCode();
    var hash2 = path2.GetHashCode();

    // --assert
    hash1.Should().Be(hash2);
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void RelativePath_GetHashCode_should_be_case_sensitive()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/FILE.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var hash1 = path1.GetHashCode();
    var hash2 = path2.GetHashCode();

    // --assert
    hash1.Should().NotBe(hash2);
  }

  [Test]
  public void RelativePath_OperatorEquality_should_be_true_for_same_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1 == path2;

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void RelativePath_OperatorEquality_should_be_false_for_different_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file1.txt");
    var path2 = RelativePath.Parse("some/path/file2.txt");

    // --act
    var result = path1 == path2;

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_OperatorInequality_should_be_false_for_same_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file.txt");
    var path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1 != path2;

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_OperatorInequality_should_be_true_for_different_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file1.txt");
    var path2 = RelativePath.Parse("some/path/file2.txt");

    // --act
    var result = path1 != path2;

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void AbsolutePath_Equals_should_be_true_for_same_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void AbsolutePath_Equals_should_be_false_for_different_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file1.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file2.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  [Platform("Win")]
  public void AbsolutePath_Equals_should_be_case_insensitive()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void AbsolutePath_Equals_should_be_case_sensitive()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void AbsolutePath_GetHashCode_should_be_same_for_equal_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var hash1 = path1.GetHashCode();
    var hash2 = path2.GetHashCode();

    // --assert
    hash1.Should().Be(hash2);
  }

  [Test]
  [Platform("Win")]
  public void AbsolutePath_GetHashCode_should_be_case_insensitive()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var hash1 = path1.GetHashCode();
    var hash2 = path2.GetHashCode();

    // --assert
    hash1.Should().Be(hash2);
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void AbsolutePath_GetHashCode_should_be_case_sensitive()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var hash1 = path1.GetHashCode();
    var hash2 = path2.GetHashCode();

    // --assert
    hash1.Should().NotBe(hash2);
  }

  [Test]
  public void AbsolutePath_OperatorEquality_should_be_true_for_same_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1 == path2;

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void AbsolutePath_OperatorEquality_should_be_false_for_different_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file1.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file2.txt"));

    // --act
    var result = path1 == path2;

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void AbsolutePath_OperatorInequality_should_be_false_for_same_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1 != path2;

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void AbsolutePath_OperatorInequality_should_be_true_for_different_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file1.txt"));
    var path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file2.txt"));

    // --act
    var result = path1 != path2;

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void AbsolutePath_GetTempDirectory_should_return_valid_path()
  {
    // --act
    var tempDir = AbsolutePath.GetTempDirectory();

    // --assert
    tempDir.Should().NotBeNull();
    tempDir.DirectoryExist().Should().BeTrue();
    Path.IsPathRooted(tempDir.FullPath).Should().BeTrue();
  }

  [Test]
  public void AbsolutePathExtension_SetLifetime_should_delete_file_on_lifetime_termination()
  {
    // --arrange
    AbsolutePath tempFile;

    using(var lifetime = Lifetime.CreateDisposable())
    {
      tempFile = AbsolutePath.GetTempDirectory() / "test-file.txt";
      tempFile.CreateFile();

      // --act
      tempFile.SetLifetime(lifetime);

      // --assert
      tempFile.FileExist().Should().BeTrue();
    }

    // --assert
    tempFile.FileExist().Should().BeFalse();
  }

  [Test]
  public void AbsolutePathExtension_SetLifetime_should_delete_directory_on_lifetime_termination()
  {
    // --arrange
    AbsolutePath tempDir;

    using(var lifetime = Lifetime.CreateDisposable())
    {
      tempDir = AbsolutePath.GetTempDirectory() / "test-directory";
      tempDir.CreateDirectory();

      // --act
      tempDir.SetLifetime(lifetime);

      // --assert
      tempDir.DirectoryExist().Should().BeTrue();
    }

    // --assert
    tempDir.DirectoryExist().Should().BeFalse();
  }

  [Test]
  public void AbsolutePathExtension_SetLifetime_should_return_same_path_for_chaining()
  {
    // --arrange
    using var lifetime = Lifetime.CreateDisposable();
    var tempFile = AbsolutePath.GetTempDirectory() / "test-file.txt";
    tempFile.CreateFile();

    // --act
    var result = tempFile.SetLifetime(lifetime);

    // --assert
    result.Should().BeSameAs(tempFile);
  }

  [Test]
  public void AbsolutePath_Equals_object_should_be_true_for_same_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void AbsolutePath_Equals_object_should_be_false_for_different_paths()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file1.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file2.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void AbsolutePath_Equals_object_should_be_false_for_null()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object? nullObj = null;

    // --act
    var result = path.Equals(nullObj);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void AbsolutePath_Equals_object_should_be_false_for_different_type()
  {
    // --arrange
    var path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object obj = "some/path/file.txt";

    // --act
    var result = path.Equals(obj);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void AbsolutePath_Equals_object_should_be_false_for_relative_path()
  {
    // --arrange
    var absolutePath = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object relativePath = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = absolutePath.Equals(relativePath);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_Equals_object_should_be_true_for_same_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file.txt");
    object path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void RelativePath_Equals_object_should_be_false_for_different_paths()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/file1.txt");
    object path2 = RelativePath.Parse("some/path/file2.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_Equals_object_should_be_false_for_null()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");
    object? nullObj = null;

    // --act
    var result = path.Equals(nullObj);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_Equals_object_should_be_false_for_different_type()
  {
    // --arrange
    var path = RelativePath.Parse("some/path/file.txt");
    object obj = "some/path/file.txt";

    // --act
    var result = path.Equals(obj);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void RelativePath_Equals_object_should_be_false_for_absolute_path()
  {
    // --arrange
    var relativePath = RelativePath.Parse("some/path/file.txt");
    object absolutePath = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = relativePath.Equals(absolutePath);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  [Platform("Win")]
  public void RelativePath_Equals_object_should_be_case_insensitive()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/FILE.txt");
    object path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void RelativePath_Equals_object_should_be_case_sensitive()
  {
    // --arrange
    var path1 = RelativePath.Parse("some/path/FILE.txt");
    object path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  [Platform("Win")]
  public void AbsolutePath_Equals_object_should_be_case_insensitive()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void AbsolutePath_Equals_object_should_be_case_sensitive()
  {
    // --arrange
    var path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void PathBase_Equals_object_should_be_true_for_same_absolute_paths()
  {
    // --arrange
    PathBase path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void PathBase_Equals_object_should_be_true_for_same_relative_paths()
  {
    // --arrange
    PathBase path1 = RelativePath.Parse("some/path/file.txt");
    object path2 = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void PathBase_Equals_object_should_be_false_for_different_paths()
  {
    // --arrange
    PathBase path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/file1.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file2.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void PathBase_Equals_object_should_be_false_for_null()
  {
    // --arrange
    PathBase path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object? nullObj = null;

    // --act
    var result = path.Equals(nullObj);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void PathBase_Equals_object_should_be_false_for_different_type()
  {
    // --arrange
    PathBase path = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object obj = "some/path/file.txt";

    // --act
    var result = path.Equals(obj);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void PathBase_Equals_object_should_be_false_for_mixed_path_types()
  {
    // --arrange
    PathBase absolutePath = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));
    object relativePath = RelativePath.Parse("some/path/file.txt");

    // --act
    var result = absolutePath.Equals(relativePath);

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  [Platform("Win")]
  public void PathBase_Equals_object_should_be_case_insensitive()
  {
    // --arrange
    PathBase path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  [Platform("Linux, Unix, MacOsX")]
  public void PathBase_Equals_object_should_be_case_sensitive()
  {
    // --arrange
    PathBase path1 = AbsolutePath.Parse(Path.GetFullPath("some/path/FILE.txt"));
    object path2 = AbsolutePath.Parse(Path.GetFullPath("some/path/file.txt"));

    // --act
    var result = path1.Equals(path2);

    // --assert
    result.Should().BeFalse();
  }
}