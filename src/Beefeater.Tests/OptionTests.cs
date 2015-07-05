using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

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

                _option = new Option<Foo>(_foo);
            }

            [Fact]
            public void ValueOrNullMatchesOriginalFoo()
            {
                Assert.Equal(_foo, _option.ValueOr(null));
            }

            [Fact]
            public void ValueOrNewFooMatchesOriginalFoo()
            {
                Assert.Equal(_foo, _option.ValueOr(new Foo()));
            }

            [Fact]
            public void ActionMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);

                var someCalledButNoneNotCalled = someCalled && !noneCalled;
                Assert.True(someCalledButNoneNotCalled);
            }

            [Fact]
            public void ActionMatchWithNullSomeCaseThrowsException()
            {
                Action<Action<Foo>,Action> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(null, () => { }).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithNullNoneCaseThrowsException()
            {
                Action<Action<Foo>, Action> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(v => { },null).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithBothCasesNullThrowsException()
            {
                Action<Action<Foo>, Action> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(null, null).AsThrowsDelegate());
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
            public void ValueOrNullMatchesNull()
            {
                Assert.Equal(null, _option.ValueOr(null));
            }

            [Fact]
            public void ValueOrNewFooMatchesNewFoo()
            {
                var newFoo = new Foo();
                Assert.Equal(newFoo, _option.ValueOr(newFoo));
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                     v => someCalled = true,
                     () => noneCalled = true);

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }

            [Fact]
            public void ActionMatchWithNullSomeCaseThrowsException()
            {
                Action<Action<Foo>, Action> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(null, () => { }).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithNullNoneCaseThrowsException()
            {
                Action<Action<Foo>, Action> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(v => { }, null).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithBothCasesNullThrowsException()
            {
                Action<Action<Foo>, Action> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(null, null).AsThrowsDelegate());
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
            public void ValueOrNullMatchesNull()
            {
                Assert.Equal(null, _option.ValueOr(null));
            }

            [Fact]
            public void ValueOrNewFooMatchesNewFoo()
            {
                var newFoo = new Foo();
                Assert.Equal(newFoo, _option.ValueOr(newFoo));
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }
        }

        public class WhenAnOptionWithAnIntIsConstructedThen
        {
            private readonly int _value;
            private readonly Option<int> _option;

            public WhenAnOptionWithAnIntIsConstructedThen()
            {
                _value = 42;

                _option = new Option<int>(_value);
            }

            [Fact]
            public void ValueOrMinMatchesOriginalFoo()
            {
                Assert.Equal(_value, _option.ValueOr(int.MinValue));
            }
            
            [Fact]
            public void ActionMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);

                var someCalledButNoneNotCalled = someCalled && !noneCalled;
                Assert.True(someCalledButNoneNotCalled);
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
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }
        }

        public class WhenAnIntOptionIsConstructedWithANullableIntThen
        {
            private readonly int? _value;
            private readonly Option<int> _option;

            public WhenAnIntOptionIsConstructedWithANullableIntThen()
            {
                _value = 42;

                _option = _value.AsAnOption();
            }

            [Fact]
            public void ValueOrMinMatchesOriginalFoo()
            {
                Assert.Equal(_value, _option.ValueOr(int.MinValue));
            }
        }

        public class WhenAnOptionWithANullableIntIsConstructedThen
        {
            private readonly int _value;
            private readonly Option<int?> _option;

            public WhenAnOptionWithANullableIntIsConstructedThen()
            {
                _value = 42;

                _option = new Option<int?>(_value);
            }

            [Fact]
            public void ValueOrMinMatchesOriginalFoo()
            {
                Assert.Equal(_value, _option.ValueOr(int.MinValue));
            }

            [Fact]
            public void CanUnboxIntoAnOptionInt()
            {
                Option<int> unboxed = _option.Unbox();
                Assert.Equal(_value, unboxed.ValueOr(int.MinValue));
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);
                
                var someCalledButNoneNotCalled = someCalled && !noneCalled;
                Assert.True(someCalledButNoneNotCalled);
            }
        }

        public class WhenCreatingANoneOfOptionNullableIntThen
        {
            private readonly Option<int?> _option;

            public WhenCreatingANoneOfOptionNullableIntThen()
            {
                _option = Option<int?>.None;
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
            public void CanUnboxIntoAnOptionInt()
            {
                Option<int> unboxed = _option.Unbox();
                Assert.Equal(int.MinValue, unboxed.ValueOr(int.MinValue));
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }
        }

        public class WhenCreatingAOptionNullableIntWhenNullThen
        {
            private readonly Option<int?> _option;

            public WhenCreatingAOptionNullableIntWhenNullThen()
            {
                _option = new Option<int?>(null);
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
            public void CanUnboxIntoAnOptionInt()
            {
                Option<int> unboxed = _option.Unbox();
                Assert.Equal(int.MinValue, unboxed.ValueOr(int.MinValue));
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: () => noneCalled = true);

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }
        }

        public class Foo
        {
        }
    }
}
