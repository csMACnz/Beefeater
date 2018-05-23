using System;
using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.EitherTests
{
    public class WhenCreatingAnEitherFooBarUsingNullFooThen
    {
        private readonly Either<Foo, Bar> _either;

        public WhenCreatingAnEitherFooBarUsingNullFooThen()
        {
            _either = Either<Foo, Bar>.OfResult1(null);
        }

        [Fact]
        public void HasValueReturnsFalse()
        {
            Assert.False(_either.HasValue);
        }

        [Fact]
        public void AccessingItem1ReturnsNull()
        {
            Assert.Equal(null, _either.Item1);
        }

        [Fact]
        public void ExplicitCastToAnFooThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _either.Item2);
        }

        public class Foo
        {
        }

        public class Bar
        {
        }
    }
}