using System;
using System.Diagnostics;
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
        public void ExplicitCastToAnIntThrowsException()
        {
            Assert.Throws<PanicException>(() => (int)_option);
        }

        [Fact]
        public void CanExplicitCastBackToANullableInt()
        {
            Assert.Throws<PanicException>(() => (int?)_option);
        }

        public class Foo
        {
        }
    }
}