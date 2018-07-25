using System;
using System.Diagnostics.CodeAnalysis;
using BCLExtensions;
using Xunit;

namespace Beefeater.Tests
{
    public class NotNullTests
    {
        [Fact]
        public void ValidFooConstructsNotNullCorrectly()
        {
            var foo = new Foo();

            var item = CreateNotNull(foo);

            Assert.Equal(foo, GetValue(item));
        }

        [Fact]
        public void ValidFooCanImplicityCastToNotNullOfFoo()
        {
            Foo foo = new Foo();

            NotNull<Foo> result = foo;

            Assert.Equal(foo, result.Value);
        }

        [Fact]
        public void ValidFooConstructedNotNullCanImplicityCastBackToFoo()
        {
            Foo foo = new Foo();

            NotNull<Foo> item = CreateNotNull(foo);

            Foo result = item;

            Assert.Equal(foo, result);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull", Justification = "Test")]
        public void NullReferenceThrowsException()
        {
            const Foo foo = null;

            Func<Foo, NotNull<Foo>> action = CreateNotNull;
            Assert.Throws<PanicException>(action.AsActionUsing(foo));
        }

        [Fact]
        public void DefaultNotNullThrowsWhenValueIsAccessed()
        {
            var notNullFoo = default(NotNull<Foo>);

            Func<NotNull<Foo>, Foo> action = GetValue;
            Assert.Throws<PanicException>(action.AsActionUsing(notNullFoo));
        }

        [Fact]
        public void NotNullUsingEmptyConstructorThrowsWhenValueIsAccessed()
        {
            var notNullFoo = new NotNull<Foo>();

            Func<NotNull<Foo>, Foo> action = GetValue;
            Assert.Throws<PanicException>(action.AsActionUsing(notNullFoo));
        }

        private static NotNull<Foo> CreateNotNull(Foo foo)
        {
            var item = new NotNull<Foo>(foo);
            return item;
        }

        private static Foo GetValue(NotNull<Foo> item)
        {
            return item.Value;
        }

        public class Foo
        {
        }
    }
}
