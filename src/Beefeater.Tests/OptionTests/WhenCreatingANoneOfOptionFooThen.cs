using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenCreatingANoneOfOptionFooThen
    {
        private readonly Option<Foo> _option;

        public WhenCreatingANoneOfOptionFooThen()
        {
            _option = Option<Foo>.None;
        }

        [Fact]
        public void ValueOrNullMatchesNull()
        {
            Assert.Equal(null, _option.ValueOr(null));
        }

        [Fact]
        public void ValueOrNewFooMatchesNewFoo()
        {
            var newFoo = new Foo();
            Assert.Equal(newFoo, _option.ValueOr(newFoo));
        }

        [Fact]
        public void ActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;

            _option.Match(
                some: v => someCalled = true,
                none: () => noneCalled = true);

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        public class Foo
        {
        }
    }
}