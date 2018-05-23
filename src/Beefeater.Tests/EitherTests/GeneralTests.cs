using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Beefeater.Tests.EitherTests
{
    public class GeneralTests
    {
        [Fact]
        public void ValidFooCanImplicityCastToEitherOfFooBar()
        {
            Foo foo = new Foo();

            Either<Foo,Bar> result = foo;

            Assert.Equal(foo, result.Item1);
        }
[Fact]
        public void ValidBarCanImplicityCastToEitherOfFooBar()
        {
            Bar bar = new Bar();

            Either<Foo,Bar> result = bar;

            Assert.Equal(bar, result.Item2);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void NullFooCanImplicityCastToNoneEitherOfFooBar()
        {
            Foo foo = null;

            Either<Foo,Bar> result = foo;

            Assert.False(result.HasValue);
            Assert.Null(result.Item1);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void NullBarCanImplicityCastToNoneEitherOfFooBar()
        {
            Bar bar = null;

            Either<Foo,Bar> result = bar;

            Assert.False(result.HasValue);
            Assert.Null(result.Item2);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void UsingBarEitherOfFooBarHasCorrectState()
        {
            Bar bar = new Bar();

            Either<Foo,Bar> result = bar;

            Assert.True(result.HasValue);
            Assert.False(result.IsItem1);
            Assert.True(result.IsItem2);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void UsingFooEitherOfFooBarHasCorrectState()
        {
            Foo foo = new Foo();

            Either<Foo,Bar> result = foo;

            Assert.True(result.HasValue);
            Assert.True(result.IsItem1);
            Assert.False(result.IsItem2);
        }

        public class Foo { }
        public class Bar { }
    }
}
