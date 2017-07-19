using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Beefeater.Tests.OptionExtensionsTests
{
    public class AsAnOptionTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnIntAsAnOptionValueOrMinMatchesOriginalValue(int expected)
        {
            int? value = expected;

            var option = value.AsAnOption();

            Assert.Equal(expected, option.ValueOr(int.MinValue));
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void GivenANullIntAsAnOptionValueOrMinMatchesOriginalValue()
        {
            int? value = null;

            var option = value.AsAnOption();

            Assert.Equal(int.MinValue, option.ValueOr(int.MinValue));
        }
    }
}