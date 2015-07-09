using System;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenCreatingAOptionNullableIntWhenNullThen
    {
        private readonly Option<int?> _option;

        public WhenCreatingAOptionNullableIntWhenNullThen()
        {
            _option = new Option<int?>(null);
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
        [Trait("Category", "NotOnMono")]
        public void ExplicitCastToAnIntThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => (int)_option);
        }

        [Fact]
        public void CanExplicitCastBackToANullableInt()
        {
            var result = (int?)_option;

            Assert.Equal(null, result);
        }

        public class Foo
        {
        }
    }
}