using Xunit;
using Xunit.Extensions;

namespace Beefeater.Tests.OptionExtensionsTests
{
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

