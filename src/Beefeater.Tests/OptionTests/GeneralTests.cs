﻿using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Beefeater.Tests.OptionTests
{
    public class GeneralTests
    {
        [Fact]
        public void ValidFooCanImplicityCastToOptionOfFoo()
        {
            Foo foo = new Foo();

            Option<Foo> result = foo;

            Assert.Equal(foo, (Foo)result);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void NullFooCanImplicityCastToNoneOptionOfFoo()
        {
            Foo foo = null;

            Option<Foo> result = foo;

            Assert.False(result.HasValue);
        }

        public class Foo { }
    }
}
