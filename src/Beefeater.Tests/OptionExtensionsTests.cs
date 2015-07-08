using Xunit;
using Xunit.Extensions;

namespace Beefeater.Tests
{
    public class OptionExtensionsTests
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

        public class ToNullableTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(42)]
            [InlineData(-12)]
            [InlineData(int.MinValue)]
            [InlineData(int.MaxValue)]
            public void GivenAValueToNullableResultHasValue(int expected)
            {
                var o = new Option<int>(expected);

                var nullable = o.ToNullable();

                Assert.True(nullable.HasValue && nullable.Value == expected);
            }
        }
    }
}
