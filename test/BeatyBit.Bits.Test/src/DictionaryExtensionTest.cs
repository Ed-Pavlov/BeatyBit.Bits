using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class DictionaryExtensionTest
{
  [Test]
  public void get_value_safe_should_return_value()
  {
    // --arrange
    const string key      = "Key1";
    const int    expected = 1;

    var dictionary = new Dictionary<string, int> { { key, expected } };

    // --act
    var actual = dictionary.GetValueSafe(key, defaultValue: 0);

    // --assert
    actual.Should().Be(expected);
  }

  [Test]
  public void get_value_safe_should_return_default_value_when_key_not_found()
  {
    // --arrange
    const string key = "Key1";
    const int    expected = 1;

    var dictionary = new Dictionary<string, int>();

    // --act
    var actual = dictionary.GetValueSafe(key, defaultValue: expected);

    // --assert
    actual.Should().Be(expected);
  }

  [Test]
  public void get_or_create_value_should_return_existing_value()
  {
    // --arrange
    const string key      = "Key1";
    const int    expected = 1;

    var dictionary = new Dictionary<string, int> { { key, expected } };

    // --act
    var actual = dictionary.GetOrCreateValue(key, () => 0);

    // --assert
    actual.Should().Be(expected);
  }

  [Test]
  public void get_or_create_value_should_create_and_return_new_value()
  {
    // --arrange
    const string key      = "Key1";
    const int    expected = 42;

    var dictionary = new Dictionary<string, int>();

    // --act
    var actual = dictionary.GetOrCreateValue(key, () => expected);

    // --assert
    actual.Should().Be(expected);
    dictionary[key].Should().Be(expected);
  }

  [Test]
  public void get_value_safe_should_throw_exception_for_null_dictionary()
  {
    // --arrange
    Dictionary<string, int>? dictionary = null;

    // --act
    Action getValueSafe = () => dictionary!.GetValueSafe("Key1");

    // --assert
    getValueSafe.Should().Throw<ArgumentNullException>();
  }

  [Test]
  public void get_or_create_value_should_throw_exception_for_null_dictionary()
  {
    // --arrange
    Dictionary<string, int>? dictionary = null;

    // --act
    Action getOrCreateValue = () => dictionary!.GetOrCreateValue("Key1", () => 42);

    // --assert
    getOrCreateValue.Should().Throw<ArgumentNullException>();
  }

  [Test]
  public void get_or_create_value_should_throw_exception_for_null_creation_function()
  {
    // --arrange
    var dictionary = new Dictionary<string, int>();

    // --act
    Action getOrCreateValue = () => dictionary.GetOrCreateValue("Key1", null!);

    // --assert
    getOrCreateValue.Should().Throw<ArgumentNullException>();
  }
}