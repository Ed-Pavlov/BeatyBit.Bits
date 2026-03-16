using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class StringExtensionTest
{
  [Test]
  public void is_nothing_should_return_true_for_null()
  {
    // --arrange
    string? value = null;

    // --act
    var result = value.IsNothing();

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void is_nothing_should_return_true_for_empty_string()
  {
    // --arrange
    var value = string.Empty;

    // --act
    var result = value.IsNothing();

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void is_nothing_should_return_true_for_whitespace()
  {
    // --arrange
    var value = "   ";

    // --act
    var result = value.IsNothing();

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void is_nothing_should_return_true_for_single_space()
  {
    // --arrange
    var value = " ";

    // --act
    var result = value.IsNothing();

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void is_nothing_should_return_false_for_non_empty_string()
  {
    // --arrange
    var value = "test";

    // --act
    var result = value.IsNothing();

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void is_nothing_should_return_false_for_string_with_content_and_whitespace()
  {
    // --arrange
    var value = " test ";

    // --act
    var result = value.IsNothing();

    // --assert
    result.Should().BeFalse();
  }
}
