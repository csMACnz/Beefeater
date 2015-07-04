using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;

namespace Beefeater.Tests
{
    public class OptionTests
    {
        public class WhenAnOptionWithAValidFooIsConstructedThen
        {
            private readonly Foo _foo;
            private readonly Option<Foo> _option;

            public WhenAnOptionWithAValidFooIsConstructedThen()
            {
                _foo = new Foo();

                _option = CreateOption(_foo);
            }

            [Fact]
            public void OptionHasValueIsTrue()
            {
                Assert.True(_option.HasValue);
            }

            [Fact]
            public void OptionValueMatchesOriginalFoo()
            {
                Assert.Equal(_foo, GetValue(_option));
            }
        }

        public class WhenCreatingAnOptionFooUsingNullThen
        {
            private readonly Option<Foo> _option;

            public WhenCreatingAnOptionFooUsingNullThen()
            {
                _option = new Option<Foo>(null);
            }

            [Fact]
            public void OptionHasValueIsTrue()
            {
                Assert.False(_option.HasValue);
            }

            [Fact]
            public void GetValueThrowsException()
            {
                Func<Option<Foo>, Foo> action = GetValue;
                Assert.Throws<PanicException>(action.AsActionUsing(_option).AsThrowsDelegate());
            }
        }

        public class WhenCreatingANoneOfOptionFooThen
        {
            private readonly Option<Foo> _option;

            public WhenCreatingANoneOfOptionFooThen()
            {
                _option = Option<Foo>.None;
            }

            [Fact]
            public void OptionHasValueIsTrue()
            {
                Assert.False(_option.HasValue);
            }

            [Fact]
            public void GetValueThrowsException()
            {
                Func<Option<Foo>, Foo> action = GetValue;
                Assert.Throws<PanicException>(action.AsActionUsing(_option).AsThrowsDelegate());
            }
        }

        public class WhenAnOptionWithAnIntIsConstructedThen
        {
            private readonly int _value;
            private readonly Option<int> _option;

            public WhenAnOptionWithAnIntIsConstructedThen()
            {
                _value = 42;

                _option = CreateOption(_value);
            }

            [Fact]
            public void OptionHasValueIsTrue()
            {
                Assert.True(_option.HasValue);
            }

            [Fact]
            public void OptionValueMatchesOriginalFoo()
            {
                Assert.Equal(_value, GetValue(_option));
            }
        }

        public class WhenCreatingANoneOfOptionIntThen
        {
            private readonly Option<int> _option;

            public WhenCreatingANoneOfOptionIntThen()
            {
                _option = Option<int>.None;
            }

            [Fact]
            public void OptionHasValueIsTrue()
            {
                Assert.False(_option.HasValue);
            }

            [Fact]
            public void GetValueThrowsException()
            {
                Func<Option<int>, int> action = GetValue;
                Assert.Throws<PanicException>(action.AsActionUsing(_option).AsThrowsDelegate());
            }
        }

        private static T GetValue<T>(Option<T> item)
        {
            return item.Value;
        }

        private static Option<T> CreateOption<T>(T foo)
        {
            return new Option<T>(foo);
        }

        public class Foo
        {
        }
    }
}
