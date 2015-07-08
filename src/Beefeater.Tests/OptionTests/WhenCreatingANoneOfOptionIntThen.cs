using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenCreatingANoneOfOptionIntThen
    {
        private readonly Option<int> _option;

        public WhenCreatingANoneOfOptionIntThen()
        {
            _option = Option<int>.None;
        }

        [Fact]
        public void ValueOrMinMatchesMin()
        {
            Assert.Equal(int.MinValue, _option.ValueOr(int.MinValue));
        }

        [Fact]
        public void ValueOrMaxMatchesMax()
        {
            Assert.Equal(int.MaxValue, _option.ValueOr(int.MaxValue));
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
    }
}