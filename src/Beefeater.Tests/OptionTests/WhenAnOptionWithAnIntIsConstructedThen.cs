﻿using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenAnOptionWithAnIntIsConstructedThen
    {
        private readonly int _value;
        private Option<int> _option;

        public WhenAnOptionWithAnIntIsConstructedThen()
        {
            _value = 42;

            _option = new Option<int>(_value);
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

        [Fact]
        public void CanExplicitCastBackToANullableInt()
        {
            var result = (int?)_option;

            Assert.Equal(_value, result);
        }
    }
}