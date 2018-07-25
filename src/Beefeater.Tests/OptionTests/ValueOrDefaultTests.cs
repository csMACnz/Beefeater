using Xunit;

namespace Beefeater.Tests.OptionTests
{
    public class ValueOrDefaultTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnOptionOfIntValueOrDefaultMatchesOriginalInt(int value)
        {
            var option = new Option<int>(value);

            Assert.Equal(value, option.ValueOrDefault());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnOptionOfNullableIntValueOrDefaultMatchesOriginalInt(int value)
        {
            var option = new Option<int?>(value);

            Assert.Equal(value, option.ValueOrDefault());
        }

        [Fact]
        public void GivenOptionOfFooValueOrDefaultMatchesOriginalFoo()
        {
            var foo = new Foo();

            var option = new Option<Foo>(foo);

            Assert.Equal(foo, option.ValueOrDefault());
        }

        [Fact]
        public void GivenNoneOptionOfFooValueOrDefaultReturnsNull()
        {
            var option = Option<Foo>.None;

            Assert.Equal(null, option.ValueOrDefault());
        }

        [Fact]
        public void GivenNoneOptionOfIntValueOrDefaultReturnsZero()
        {
            var option = Option<int>.None;

            Assert.Equal(0, option.ValueOrDefault());
        }

        [Fact]
        public void GivenNoneOptionOfNullableIntValueOrDefaultReturnsNullNullable()
        {
            var option = Option<int?>.None;

            Assert.Equal(null, option.ValueOrDefault());
        }

        [Fact]
        public void GivenAnOptionFooCreatedUsingNullValueOrDefaultMatchesOriginalFoo()
        {
            var option = new Option<Foo>(null);

            Assert.Equal(null, option.ValueOrDefault());
        }

        [Fact]
        public void GivenAnOptionNullableIntCreatedUsingNullValueOrDefaultReturnsNullNullable()
        {
            var option = new Option<int?>(null);
            Assert.Equal(null, option.ValueOrDefault());
        }

        public class Foo
        {
        }
    }
}