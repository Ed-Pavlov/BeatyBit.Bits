using System.Collections;
using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class EmptyTest
{
  [Test]
  public void enumerator_should_return_same_instance()
  {
    // --arrange & act
    var first  = Empty.Enumerator;
    var second = Empty.Enumerator;

    // --assert
    first.Should().BeSameAs(second);
  }

  [Test]
  public void enumerator_move_next_should_return_false()
  {
    // --arrange
    var enumerator = Empty.Enumerator;

    // --act
    var result = enumerator.MoveNext();

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void enumerator_current_should_be_null()
  {
    // --arrange
    var enumerator = Empty.Enumerator;

    // --act
    var current = enumerator.Current;

    // --assert
    current.Should().BeNull();
  }

  [Test]
  public void enumerator_reset_should_not_throw()
  {
    // --arrange
    var enumerator = Empty.Enumerator;

    // --act
    Action reset = () => enumerator.Reset();

    // --assert
    reset.Should().NotThrow();
  }

  [Test]
  public void generic_empty_list_should_return_same_instance()
  {
    // --arrange & act
    var first  = Empty<int>.List;
    var second = Empty<int>.List;

    // --assert
    first.Should().BeSameAs(second);
  }

  [Test]
  public void generic_empty_list_should_be_empty()
  {
    // --arrange & act
    var list = Empty<string>.List;

    // --assert
    list.Should().BeEmpty();
    list.Count.Should().Be(0);
  }

  [Test]
  public void generic_empty_list_should_be_different_for_different_types()
  {
    // --arrange & act
    var intList    = Empty<int>.List;
    var stringList = Empty<string>.List;

    // --assert
    ((object)intList).Should().NotBeSameAs(stringList);
  }
}
