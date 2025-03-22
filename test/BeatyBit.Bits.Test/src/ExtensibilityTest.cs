using BeatyBit.Bits.Extensibility;
using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class ExtensibilityTest
{
  [Test]
  public void test_class_implementing_internals()
  {
    // --arrange
    var target = new Subject();

    // --act
    // ReSharper disable once RedundantTypeArgumentsOfMethod
    var internals = target.GetInternals<string, string, string, string, string, string, string>();

    // --assert
    internals.Member1.Should().Be("1");
    internals.Member2.Should().Be("2");
    internals.Member3.Should().Be("3");
    internals.Member4.Should().Be("4");
    internals.Member5.Should().Be("5");
    internals.Member6.Should().Be("6");
    internals.Member7.Should().Be("7");
  }

  [Test]
  public void test_class_interface_of_class_implementing_internals()
  {
    // --arrange
    ISubject target = new Subject();

    // --act
    var internals = target.GetInternals<string, string, string, string, string, string, string>();

    // --assert
    internals.Member1.Should().Be("1");
    internals.Member2.Should().Be("2");
    internals.Member3.Should().Be("3");
    internals.Member4.Should().Be("4");
    internals.Member5.Should().Be("5");
    internals.Member6.Should().Be("6");
    internals.Member7.Should().Be("7");
  }


  [Test]
  public void get_internals_safe_should_return_true_when_class_implementing_internals()
  {
    // --arrange
    var target = new Subject();

    // --act
    target.GetInternalsSafe<string>(out var internal1).Should().BeTrue();
    target.GetInternalsSafe<string, string>(out var internal2).Should().BeTrue();
    target.GetInternalsSafe<string, string, string>(out var internal3).Should().BeTrue();
    target.GetInternalsSafe<string, string, string, string>(out var internal4).Should().BeTrue();
    target.GetInternalsSafe<string, string, string, string, string>(out var internal5).Should().BeTrue();
    target.GetInternalsSafe<string, string, string, string, string, string>(out var internal6).Should().BeTrue();
    target.GetInternalsSafe<string, string, string, string, string, string, string>(out var internal7).Should().BeTrue();

    // --assert
    internal1!.Member1.Should().Be("1");
    internal2!.Member2.Should().Be("2");
    internal3!.Member3.Should().Be("3");
    internal4!.Member4.Should().Be("4");
    internal5!.Member5.Should().Be("5");
    internal6!.Member6.Should().Be("6");
    internal7!.Member7.Should().Be("7");
  }

  [Test]
  public void get_internals_safe_should_return_false_when_class_not_implementing_internals()
  {
    // --arrange
    var target = new BadSubject();

    // --act, assert
    target.GetInternalsSafe<string>(out var internal1).Should().BeFalse();
    target.GetInternalsSafe<string, string>(out var internal2).Should().BeFalse();
    target.GetInternalsSafe<string, string, string>(out var internal3).Should().BeFalse();
    target.GetInternalsSafe<string, string, string, string>(out var internal4).Should().BeFalse();
    target.GetInternalsSafe<string, string, string, string, string>(out var internal5).Should().BeFalse();
    target.GetInternalsSafe<string, string, string, string, string, string>(out var internal6).Should().BeFalse();
    target.GetInternalsSafe<string, string, string, string, string, string, string>(out var internal7).Should().BeFalse();

    internal1.Should().BeNull();
    internal2.Should().BeNull();
    internal3.Should().BeNull();
    internal4.Should().BeNull();
    internal5.Should().BeNull();
    internal6.Should().BeNull();
    internal7.Should().BeNull();
  }

  [Test]
  public void class_not_implementing_internals_should_throw_exception()
  {
    // --arrange
    var target = new BadSubject();

    var getInternals1 = () => target.GetInternals<string>();
    var getInternals2 = () => target.GetInternals<string, string>();
    var getInternals3 = () => target.GetInternals<string, string, string>();
    var getInternals4 = () => target.GetInternals<string, string, string, string>();
    var getInternals5 = () => target.GetInternals<string, string, string, string, string>();
    var getInternals6 = () => target.GetInternals<string, string, string, string, string, string>();
    var getInternals7 = () => target.GetInternals<string, string, string, string, string, string, string>();

    // --assert
    getInternals1.Should().Throw<ArgumentException>();
    getInternals2.Should().Throw<ArgumentException>();
    getInternals3.Should().Throw<ArgumentException>();
    getInternals4.Should().Throw<ArgumentException>();
    getInternals5.Should().Throw<ArgumentException>();
    getInternals6.Should().Throw<ArgumentException>();
    getInternals7.Should().Throw<ArgumentException>();
  }

  private interface ISubject;

  private class Subject : ISubject, IInternal<string, string, string, string, string, string, string>
  {
    string IInternal<string>.                                                Member1 => "1";
    string IInternal<string, string>.                                        Member2 => "2";
    string IInternal<string, string, string>.                                Member3 => "3";
    string IInternal<string, string, string, string>.                        Member4 => "4";
    string IInternal<string, string, string, string, string>.                Member5 => "5";
    string IInternal<string, string, string, string, string, string>.        Member6 => "6";
    string IInternal<string, string, string, string, string, string, string>.Member7 => "7";
  }

  private class BadSubject : ISubject;
}