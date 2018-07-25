using Xunit;

namespace Beefeater.Tests.OptionExtensionsTests
{
    public class UnboxTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAValueUnboxCanUnboxIntoAnOptionInt(int expected)
        {
            var option = new Option<int?>(expected);
            Option<int> unboxed = option.Unbox();
            Assert.Equal(expected, unboxed.ValueOr(int.MinValue));
        }

        [Fact]
        public void GivenANoneUnboxCanUnboxIntoAnOptionInt()
        {
            var option = Option<int?>.None;

            Option<int> unboxed = option.Unbox();

            Assert.Equal(int.MinValue, unboxed.ValueOr(int.MinValue));
        }

        [Fact]
        public void GivenANullOptionConstructionUnboxCanUnboxIntoAnOptionInt()
        {
            var option = new Option<int?>(null);

            Option<int> unboxed = option.Unbox();

            Assert.Equal(int.MinValue, unboxed.ValueOr(int.MinValue));
        }

        [Fact]
        public void GivenADefaultOptionConstructionUnboxCanUnboxIntoAnOptionInt()
        {
            var option = default(Option<int?>);

            Option<int> unboxed = option.Unbox();

            Assert.Equal(int.MinValue, unboxed.ValueOr(int.MinValue));
        }
    }
}