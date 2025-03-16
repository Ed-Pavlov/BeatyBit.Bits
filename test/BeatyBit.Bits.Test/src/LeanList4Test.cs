using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class LeanList4Test
{
  [TestCaseSource(nameof(items_source))]
  public void add_should_store_elements(int[] expected)
  {
    // --arrange
    var list = new LeanList4<int>();

    // --act
    foreach(var item in expected)
      list.Add(item);

    // --assert
    list.Should().BeEquivalentTo(expected);
  }

  [Test]
  public void indexer_should_throw_for_invalid_index()
  {
    // --arrange
    // ReSharper disable once CollectionNeverUpdated.Local
    var list = new LeanList4<string>();

    // --act
    var get = () => { _       = list[0]; };
    var set = () => { list[0] = "test"; };

    // --assert
    get.Should().ThrowExactly<ArgumentOutOfRangeException>();
    set.Should().ThrowExactly<ArgumentOutOfRangeException>();
  }

  [TestCaseSource(nameof(items_source))]
  public void contains_should_return_true_for_existing_element(int[] expected)
  {
    // --arrange
    var list = new LeanList4<int>(expected);

    // --act
    var result = list.Contains(expected[2]);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void contains_should_return_true_for_existing_element_far_then_four()
  {
    // --arrange
    var list = new LeanList4<int>([1, 2, 3, 4, 5]);

    // --act
    var result = list.Contains(5);

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void indexof_should_return_correct_index()
  {
    // --arrange
    var list = new LeanList4<double> { 1.1, 2.2, 3.3 };

    // --act
    var index = list.IndexOf(2.2);

    // --assert
    index.Should().Be(1, "because the element '2.2' is at index 1 in the list");
  }

  [Test]
  public void indexof_should_return_correct_index_far_then_four()
  {
    // --arrange
    var list = new LeanList4<double>
    {
      1.1,
      2.2,
      3.3,
      4,
      5
    };

    // --act
    var index = list.IndexOf(5);

    // --assert
    index.Should().Be(4);
  }

  [TestCaseSource(nameof(items_source))]
  public void clear_should_reset_list(int[] expected)
  {
    // --arrange
    var list = new LeanList4<int>(expected);

    // --act
    list.Clear();

    // --assert
    list.Should().BeEmpty("because Clear() was called");
  }

  [TestCaseSource(nameof(items_source))]
  public void add_range_should_add_multiple_items(int[] expected)
  {
    // --arrange
    var list = new LeanList4<int>();

    // --act
    list.AddRange(expected);

    // --assert
    list.Should().BeEquivalentTo(expected);
  }

  private static IEnumerable<TestCaseData> items_source()
  {
    yield return new TestCaseData(new[] { 1, 2, 3 }).SetName("Less than 4 items");
    yield return new TestCaseData(new[] { 1, 2, 3, 4 }).SetName("Exactly 4 items");
    yield return new TestCaseData(new[] { 1, 2, 3, 4, 5 }).SetName("More than 4 items");
  }
}