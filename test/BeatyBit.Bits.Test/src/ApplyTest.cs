using BeatyBit.Bits.Extensibility;
using FakeItEasy;
using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class ApplyTest
{
  [Test]
  public void apply_should_invoke_action()
  {
    // --arrange
    var instance = new object();
    var action   = A.Fake<Action<object>>();

    // --act
    instance.Apply(action);

    // --assert
    A.CallTo(() => action(instance)).MustHaveHappenedOnceExactly();
  }

  [Test]
  public void apply_should_throw_when_action_is_null()
  {
    // --arrange
    var            instance = new object();
    Action<object> action   = null!;

    // --act
    Action apply = () => instance.Apply(action);

    // --assert
    apply.Should().Throw<ArgumentNullException>();
  }

  [Test]
  public void apply_should_invoke_action_on_value_type_instance()
  {
    // --arrange
    const int instance = 42;
    var       action   = A.Fake<Action<int>>();

    // --act
    instance.Apply(action);

    // --assert
    A.CallTo(() => action(instance)).MustHaveHappenedOnceExactly();
  }

  [Test]
  public void apply_should_work_with_reference_type_with_mutable_state()
  {
    // --arrange
    var instance = new List<int> { 1, 2, 3 };
    var action   = new Action<List<int>>(list => list.Add(4));

    // --act
    instance.Apply(action);

    // --assert
    instance.Should().Contain(4);
  }

  [Test]
  public void apply_should_throw_when_value_is_null()
  {
    // --arrange
    object value = null!;
    var action   = A.Fake<Action<object>>();

    // --act
    Action apply = () => value.Apply(action);

    // --assert
    apply.Should().Throw<ArgumentNullException>()
         .WithMessage($"Value cannot be null. (Parameter '{nameof(value)}')");
  }
}