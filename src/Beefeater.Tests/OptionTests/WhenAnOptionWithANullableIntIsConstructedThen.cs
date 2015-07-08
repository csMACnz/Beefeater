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
        public void ActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;

            _option.Match(
                some: v => someCalled = true,
                none: () => noneCalled = true);
                
            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }
    }
}