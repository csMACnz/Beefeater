using System;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenCreatingANoneOfOptionNullableIntThen
    {
        private readonly Option<int?> _option;

        public WhenCreatingANoneOfOptionNullableIntThen()
        {
            _option = Option<int?>.None;
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
        public void ValueOrDefaultReturnsNullNullable()
        {
            Assert.Equal(null, _option.ValueOrDefault());
        }
          
        [Fact]
        public void ActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;

            _option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Fact]
        public void FuncMatchReturnsExpectedNone()
        {
            var result = _option.Match(
                some: v => v,
                none: () => -1);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void ExplicitCastToAnIntThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => (int)_option);
        }

        [Fact]
        public void CanExplicitCastBackToANullableInt()
        {
            var result = (int?)_option;

            Assert.Equal(null, result);
        }

    }
}