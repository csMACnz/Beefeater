using System;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenAnOptionWithANullableIntIsConstructedThen
    {
        private readonly int _value;
        private readonly Option<int?> _option;

        public WhenAnOptionWithANullableIntIsConstructedThen()
        {
            _value = 42;

            _option = new Option<int?>(_value);
        }

        [Fact]
        public void ValueOrMinMatchesOriginalFoo()
        {
            Assert.Equal(_value, _option.ValueOr(int.MinValue));
        }

        [Fact]
        public void ValueOrDefaultMatchesOriginalFoo()
        {
            Assert.Equal(_value, _option.ValueOrDefault());
        }
          
        [Fact]
        public void ActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;

            _option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));
                
            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }

        [Fact]
        public void FuncMatchReturnsExpectedSome()
        {
            var result = _option.Match(
                some: v => v,
                none: () => null);

            Assert.Equal(_value, result);
        }
    }
}