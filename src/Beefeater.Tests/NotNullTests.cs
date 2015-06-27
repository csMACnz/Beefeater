using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
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
        public void NullReferenceThrowsException()
        {
            Foo foo = null;

            Func<Foo, NotNull<Foo>> action = CreateNotNull;
            Assert.Throws<PanicException>(action.AsActionUsing(foo).AsThrowsDelegate());
        }

        [Fact]
        public void DefaultNotNullThrowsWhenValueIsAccessed()
        {
            var notNullFoo = default(NotNull<Foo>);

            Func<NotNull<Foo>, Foo> action = GetValue;
            Assert.Throws<PanicException>(action.AsActionUsing(notNullFoo).AsThrowsDelegate());
        }

        [Fact]
        public void NotNullUsingEmptyConstructorThrowsWhenValueIsAccessed()
        {
            var notNullFoo = new NotNull<Foo>();

            Func<NotNull<Foo>, Foo> action = GetValue;
            Assert.Throws<PanicException>(action.AsActionUsing(notNullFoo).AsThrowsDelegate());
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
