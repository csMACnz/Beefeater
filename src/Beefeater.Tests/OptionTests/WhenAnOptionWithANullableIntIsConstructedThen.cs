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
        public void HasValueReturnsTrue()
        {
            Assert.True(_option.HasValue);
        }

        [Fact]
        public void ValueOrMinMatchesOriginalInt()
        {
            Assert.Equal(_value, _option.ValueOr(int.MinValue));
        }

        [Fact]
        public void CanExplicitCastBackToAnInt()
        {
            var result = (int)_option;

            Assert.Equal(_value, result);
        }

    }
}