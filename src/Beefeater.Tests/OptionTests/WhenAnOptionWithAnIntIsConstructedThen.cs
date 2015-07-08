using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenAnOptionWithAnIntIsConstructedThen
    {
        private readonly int _value;
        private readonly Option<int> _option;

        public WhenAnOptionWithAnIntIsConstructedThen()
        {
            _value = 42;

            _option = new Option<int>(_value);
        }

        [Fact]
        public void ValueOrMinMatchesOriginalFoo()
        {
            Assert.Equal(_value, _option.ValueOr(int.MinValue));
        }
            
        [Fact]
        public void ActionMatchCallsSomeButNotNone()
        {
            var noneCalled = false;
            var someCalled = false;

            _option.Match(
                some: v => someCalled = true,
                none: () => noneCalled = true);

            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }

        [Fact]
        public void FuncMatchReturnsExpectedSome()
        {
            var result = _option.Match(
                some: v => v,
                none: () => -1);

            Assert.Equal(_value, result);
        }
    }
}