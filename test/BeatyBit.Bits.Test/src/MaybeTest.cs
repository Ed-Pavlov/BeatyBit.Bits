using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class MaybeTest
{
  [Test]
  public void create_with_value_should_set_value()
  {
    // --act
    var target = new Maybe<int>(42);

    // --assert
    target.HasValue.Should().BeTrue();
    target.Value.Should().Be(42);
  }

  [Test]
  public void to_maybe_should_set_value()
  {
    // --arrange
    const int value = 42;

    // --act
    var target = value.ToMaybe();

    // --assert
    target.HasValue.Should().BeTrue();
    target.Value.Should().Be(42);
  }

  [Test]
  public void get_value_safe_should_return_default_when_Maybe_is_empty()
  {
    // --arrange
    var target = Maybe<int>.Nothing;

    // --act
    var actual = target.GetValueSafe();

    // --assert
    actual.Should().Be(default);
  }

  [Test]
  public void get_value_safe_should_return_null_when_Maybe_is_empty()
  {
    // --arrange
    var target = Maybe<string>.Nothing;

    // --act
    var actual = target.GetValueSafe();

    // --assert
    actual.Should().BeNull();
  }

  [Test]
  public void get_value_safe_should_return_value()
  {
    // --arrange
    const string expectedValue = "expected value";

    var target = expectedValue.ToMaybe();

    // --act
    var actual = target.GetValueSafe();

    // --assert
    actual.Should().Be(expectedValue);
  }

  [Test]
  public void null_value_is_valid_for_Maybe()
  {
    // --act
    var target = new Maybe<string?>(null);

    // --assert
    target.HasValue.Should().BeTrue();
    target.Value.Should().BeNull();
  }

  [Test]
  public void create_with_no_value_should_set_HasValue_to_false()
  {
    // --act
    var target = new Maybe<int>();

    // --assert
    target.HasValue.Should().BeFalse();
  }

  [Test]
  public void accessing_Value_when_Maybe_is_empty_should_throw_InvalidOperationException()
  {
    // --act
    var target = new Maybe<int>();

    // --assert
    var getValue = () => target.Value;
    getValue.Should().Throw<InvalidOperationException>();
  }

  [Test]
  public void two_Maybe_instances_with_same_value_should_be_equal()
  {
    // --arrange
    var maybe1 = new Maybe<int>(42);
    var maybe2 = new Maybe<int>(42);

    // --assert
    maybe1.Should().Be(maybe2);
  }

  [Test]
  public void two_Maybe_instances_with_different_values_should_not_be_equal()
  {
    // --arrange
    var maybe1 = new Maybe<int>(42);
    var maybe2 = new Maybe<int>(24);

    // --assert
    maybe1.Should().NotBe(maybe2);
  }

  [Test]
  public void two_empty_Maybe_instances_should_be_equal()
  {
    // --arrange
    var maybe1 = new Maybe<int>();
    var maybe2 = new Maybe<int>();

    // --assert
    maybe1.Should().Be(maybe2);
  }

  [Test]
  public void empty_Maybe_instance_should_be_equal_to_Nothing_instance()
  {
    // --arrange
    var maybe1 = new Maybe<int>();
    var maybe2 = Maybe<int>.Nothing;

    // --assert
    maybe1.Should().Be(maybe2);
  }
}