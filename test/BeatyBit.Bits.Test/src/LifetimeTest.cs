using BeatyBit.Lifetimes;
using FluentAssertions;

namespace BeatyBit.Bits.Test;

public class LifetimeTest
{
  [Test]
  public void status_should_be_alive_when_created()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    var status = definition.Status;

    // --assert
    status.Should().Be(LifetimeStatus.Alive);
  }

  [Test]
  public void status_should_be_terminated_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    definition.Terminate();

    // --assert
    definition.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void should_be_terminated_when_disposed()
  {
    // --arrange
    var definition = new LifetimeDefinition();

    // --act
    definition.Dispose();

    // --assert
    definition.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void eternal_should_be_never_terminated()
  {
    // --arrange
    var eternal = LifetimeDefinition.Eternal;

    // --act
    eternal.Terminate();

    // --assert
    eternal.Status.Should().Be(LifetimeStatus.Alive);
    eternal.IsEternal.Should().BeTrue();
  }

  [Test]
  public void terminated_should_be_terminated_when_created()
  {
    // --arrange & act
    var terminated = LifetimeDefinition.Terminated;

    // --assert
    terminated.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void lifetime_isalive_should_be_true_when_created()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    var lifetime = definition.Lifetime;

    // --assert
    lifetime.IsAlive.Should().BeTrue();
    lifetime.IsNotAlive.Should().BeFalse();
  }

  [Test]
  public void lifetime_isalive_should_be_false_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var lifetime = definition.Lifetime;

    // --act
    definition.Terminate();

    // --assert
    lifetime.IsAlive.Should().BeFalse();
    lifetime.IsNotAlive.Should().BeTrue();
  }

  [Test]
  public void lifetime_eternal_should_be_always_alive()
  {
    // --arrange & act
    var lifetime = Lifetime.Eternal;

    // --assert
    lifetime.IsAlive.Should().BeTrue();
    lifetime.IsEternal.Should().BeTrue();
  }

  [Test]
  public void lifetime_terminated_should_be_not_alive()
  {
    // --arrange & act
    var lifetime = Lifetime.Terminated;

    // --assert
    lifetime.IsAlive.Should().BeFalse();
    lifetime.IsNotAlive.Should().BeTrue();
  }

  [Test]
  public void action_should_be_executed_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var executed = false;

    // --act
    definition.Lifetime.OnTermination(() => executed = true);
    definition.Terminate();

    // --assert
    executed.Should().BeTrue();
  }

  [Test]
  public void disposable_should_be_disposed_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var disposable = new TestDisposable();

    // --act
    definition.Lifetime.OnTermination(disposable);
    definition.Terminate();

    // --assert
    disposable.IsDisposed.Should().BeTrue();
  }

  [Test]
  public void multiple_disposables_should_be_disposed_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var disposable1 = new TestDisposable();
    var disposable2 = new TestDisposable();
    var disposable3 = new TestDisposable();

    // --act
    definition.Lifetime.OnTermination(disposable1, disposable2, disposable3);
    definition.Terminate();

    // --assert
    disposable1.IsDisposed.Should().BeTrue();
    disposable2.IsDisposed.Should().BeTrue();
    disposable3.IsDisposed.Should().BeTrue();
  }

  [Test]
  public void termination_handler_should_be_called_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var handler = new TestTerminationHandler();

    // --act
    definition.Lifetime.OnTermination(handler);
    definition.Terminate();

    // --assert
    handler.WasCalled.Should().BeTrue();
  }

  [Test]
  public void resources_should_be_terminated_in_lifo_order()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var order = new List<int>();

    // --act
    definition.Lifetime.OnTermination(() => order.Add(1));
    definition.Lifetime.OnTermination(() => order.Add(2));
    definition.Lifetime.OnTermination(() => order.Add(3));
    definition.Terminate();

    // --assert
    order.Should().Equal(3, 2, 1);
  }

  [Test]
  public void tryontermination_should_return_true_when_alive()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    var result = definition.Lifetime.TryOnTermination(() => { });

    // --assert
    result.Should().BeTrue();
  }

  [Test]
  public void tryontermination_should_return_false_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var result = definition.Lifetime.TryOnTermination(() => { });

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void tryontermination_disposable_should_return_false_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var result = definition.Lifetime.TryOnTermination(new TestDisposable());

    // --assert
    result.Should().BeFalse();
  }

  [Test]
  public void ontermination_should_throw_when_already_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var act = () => definition.Lifetime.OnTermination(() => { });

    // --assert
    act.Should().Throw<InvalidOperationException>();
  }

  [Test]
  public void nested_lifetime_should_be_terminated_when_parent_terminated()
  {
    // --arrange
    using var parent = new LifetimeDefinition();
    var child = parent.Lifetime.CreateNested();

    // --act
    parent.Terminate();

    // --assert
    child.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void nested_lifetime_should_not_terminate_parent_when_terminated()
  {
    // --arrange
    using var parent = new LifetimeDefinition();
    var child = parent.Lifetime.CreateNested();

    // --act
    child.Terminate();

    // --assert
    parent.Status.Should().Be(LifetimeStatus.Alive);
    child.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void nested_lifetime_should_be_terminated_immediately_when_parent_already_terminated()
  {
    // --arrange
    using var parent = new LifetimeDefinition();
    parent.Terminate();

    // --act
    var child = parent.Lifetime.CreateNested();

    // --assert
    child.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void multiple_nested_lifetimes_should_be_terminated_when_parent_terminated()
  {
    // --arrange
    using var parent = new LifetimeDefinition();
    var child1 = parent.Lifetime.CreateNested();
    var child2 = parent.Lifetime.CreateNested();
    var child3 = parent.Lifetime.CreateNested();

    // --act
    parent.Terminate();

    // --assert
    child1.Status.Should().Be(LifetimeStatus.Terminated);
    child2.Status.Should().Be(LifetimeStatus.Terminated);
    child3.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void lifetimeDefinition_with_parent_should_be_terminated_when_parent_terminated()
  {
    // --arrange
    using var parent = new LifetimeDefinition();
    var child = new LifetimeDefinition(parent.Lifetime);

    // --act
    parent.Terminate();

    // --assert
    child.Status.Should().Be(LifetimeStatus.Terminated);
  }

  [Test]
  public void bracket_should_execute_opening_and_closing()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var openingExecuted = false;
    var closingExecuted = false;

    // --act
    definition.Lifetime.Bracket(
      () => openingExecuted = true,
      () => closingExecuted = true);

    // --assert
    openingExecuted.Should().BeTrue();
    closingExecuted.Should().BeFalse();

    definition.Terminate();
    closingExecuted.Should().BeTrue();
  }

  [Test]
  public void bracket_with_return_should_execute_opening_and_closing()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var closingValue = 0;

    // --act
    var result = definition.Lifetime.Bracket(
      () => 42,
      value => closingValue = value);

    // --assert
    result.Should().Be(42);
    closingValue.Should().Be(0);

    definition.Terminate();
    closingValue.Should().Be(42);
  }

  [Test]
  public void bracket_should_throw_when_lifetime_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var act = () => definition.Lifetime.Bracket(() => { }, () => { });

    // --assert
    act.Should().Throw<OperationCanceledException>();
  }

  [Test]
  public void bracket_with_return_should_throw_when_lifetime_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var act = () => definition.Lifetime.Bracket(() => 42, _ => { });

    // --assert
    act.Should().Throw<OperationCanceledException>();
  }

  [Test]
  public void using_should_execute_action_and_terminate()
  {
    // --arrange
    var actionExecuted = false;
    Lifetime? capturedLifetime = null;

    // --act
    Lifetime.Using(lt =>
    {
      actionExecuted = true;
      capturedLifetime = lt;
    });

    // --assert
    actionExecuted.Should().BeTrue();
    capturedLifetime.Should().NotBeNull();
    capturedLifetime!.Value.IsAlive.Should().BeFalse();
  }

  [Test]
  public async Task usingAsync_should_execute_action_and_terminate()
  {
    // --arrange
    var actionExecuted = false;
    Lifetime? capturedLifetime = null;

    // --act
    await Lifetime.UsingAsync(async lt =>
    {
      await Task.Delay(10);
      actionExecuted = true;
      capturedLifetime = lt;
    });

    // --assert
    actionExecuted.Should().BeTrue();
    capturedLifetime.Should().NotBeNull();
    capturedLifetime!.Value.IsAlive.Should().BeFalse();
  }

  [Test]
  public void executeIfAlive_should_succeed_when_alive()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    using var cookie = definition.UsingExecuteIfAlive();

    // --assert
    cookie.Succeed.Should().BeTrue();
  }

  [Test]
  public void executeIfAlive_should_fail_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    using var cookie = definition.UsingExecuteIfAlive();

    // --assert
    cookie.Succeed.Should().BeFalse();
  }

  [Test]
  public void executeIfAlive_should_increment_executing_count()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    using var cookie = definition.UsingExecuteIfAlive();

    // --assert
    definition.ExecutingCount.Should().Be(1);
  }

  [Test]
  public void executeIfAlive_should_decrement_executing_count_when_disposed()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    var cookie = definition.UsingExecuteIfAlive();
    cookie.Dispose();

    // --assert
    definition.ExecutingCount.Should().Be(0);
  }

  [Test]
  public void throwIfNotAlive_should_not_throw_when_alive()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    var act = () => definition.Lifetime.ThrowIfNotAlive();

    // --assert
    act.Should().NotThrow();
  }

  [Test]
  public void throwIfNotAlive_should_throw_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var act = () => definition.Lifetime.ThrowIfNotAlive();

    // --assert
    act.Should().Throw<OperationCanceledException>();
  }

  [Test]
  public void toCancellationToken_should_not_be_canceled_when_alive()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    var token = definition.Lifetime.ToCancellationToken();

    // --assert
    token.IsCancellationRequested.Should().BeFalse();
  }

  [Test]
  public void toCancellationToken_should_be_canceled_when_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var token = definition.Lifetime.ToCancellationToken();

    // --act
    definition.Terminate();

    // --assert
    token.IsCancellationRequested.Should().BeTrue();
  }

  [Test]
  public void implicit_conversion_should_create_cancellationToken()
  {
    // --arrange
    using var definition = new LifetimeDefinition();

    // --act
    CancellationToken token = definition.Lifetime;

    // --assert
    token.IsCancellationRequested.Should().BeFalse();
  }

  [Test]
  public void toCancellationToken_should_return_canceled_token_when_already_terminated()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    definition.Terminate();

    // --act
    var token = definition.Lifetime.ToCancellationToken();

    // --assert
    token.IsCancellationRequested.Should().BeTrue();
  }

  [Test]
  public void equality_should_work_correctly()
  {
    // --arrange
    using var definition1 = new LifetimeDefinition();
    using var definition2 = new LifetimeDefinition();
    var lifetime1a = definition1.Lifetime;
    var lifetime1b = definition1.Lifetime;
    var lifetime2 = definition2.Lifetime;

    // --act & assert
    (lifetime1a == lifetime1b).Should().BeTrue();
    (lifetime1a != lifetime2).Should().BeTrue();
    lifetime1a.Equals(lifetime1b).Should().BeTrue();
    lifetime1a.Equals(lifetime2).Should().BeFalse();
  }

  [Test]
  public void hashcode_should_be_consistent()
  {
    // --arrange
    using var definition = new LifetimeDefinition();
    var lifetime1 = definition.Lifetime;
    var lifetime2 = definition.Lifetime;

    // --act & assert
    lifetime1.GetHashCode().Should().Be(lifetime2.GetHashCode());
  }

  private class TestDisposable : IDisposable
  {
    public bool IsDisposed { get; private set; }
    public void Dispose() => IsDisposed = true;
  }

  private class TestTerminationHandler : ITerminationHandler
  {
    public bool WasCalled { get; private set; }
    public void OnTermination(Lifetime lifetime) => WasCalled = true;
  }
}