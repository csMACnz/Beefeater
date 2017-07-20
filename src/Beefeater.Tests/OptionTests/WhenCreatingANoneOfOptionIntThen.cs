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
        public void HasValueReturnsFalse()
        {
            Assert.False(_option.HasValue);
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
        public void ExplicitCastToAnIntThrowsException()
        {
            Assert.Throws<PanicException>(() => (int)_option);
        }

        [Fact]
        public void ExplicitCastToANullableIntThrowsException()
        {
            Assert.Throws<PanicException>(() => (int?)_option);
        }

    }
}