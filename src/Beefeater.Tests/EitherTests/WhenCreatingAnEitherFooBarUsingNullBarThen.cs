using System;
using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField
namespace Beefeater.Tests.EitherTests
{
    public class WhenCreatingAnEitherFooBarUsingNullBarThen
    {
        private readonly Either<Foo, Bar> _option;

        public WhenCreatingAnEitherFooBarUsingNullBarThen()
        {
            _option = Either<Foo, Bar>.OfResult2(null);
        }

        [Fact]
        public void HasValueReturnsFalse()
        {
            Assert.False(_option.HasValue);
        }

        [Fact]
        public void AccessingItem2ReturnsNull()
        {
            Assert.Equal(null, _option.Item2);
        }

        [Fact]
        public void ExplicitCastToAnFooThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _option.Item1);
        }

        public class Foo
        {
        }

        public class Bar
        {
        }
    }
}