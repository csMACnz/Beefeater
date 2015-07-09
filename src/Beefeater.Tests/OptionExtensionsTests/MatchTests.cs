using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;
using Xunit.Extensions;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionExtensionsTests
{
    public class MatchTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnOptionOfIntActionMatchCallsSomeButNotNone(int value)
        {
            var option = new Option<int>(value);

            var noneCalled = false;
            var someCalled = false;

            option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));

            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnOptionOfIntFuncMatchReturnsExpectedSome(int value)
        {
            var option = new Option<int>(value);

            var result = option.Match(
                some: v => v,
                none: () => -1);

            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnOptionOfNullableIntActionMatchCallsNoneButNotSome(int value)
        {
            var option = new Option<int?>(value);
            var noneCalled = false;
            var someCalled = false;

            option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));

            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void GivenAnOptionOfNullableIntFuncMatchReturnsExpectedSome(int value)
        {
            var option = new Option<int?>(value);

            var result = option.Match(
                some: v => v,
                none: () => null);

            Assert.Equal(value, result);
        }


        [Fact]
        public void GivenANoneOfOptionIntActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;
            var option = Option<int>.None;

            option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Fact]
        public void GivenANoneOfOptionIntFuncMatchReturnsExpectedNone()
        {
            var option = Option<int>.None;

            var result = option.Match(
                some: v => v,
                none: () => -1);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void ActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;
            var option = Option<int?>.None;

            option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Fact]
        public void FuncMatchReturnsExpectedNone()
        {
            var option = Option<int?>.None;

            var result = option.Match(
                some: v => v,
                none: () => -1);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void GivenAnOptionNullableIntWhenNullActionMatchCallsNoneButNotSome()
        {
            var noneCalled = false;
            var someCalled = false;
            var option = new Option<int?>(null);

            option.Match(
                some: v => someCalled = true,
                none: (Action)(() => noneCalled = true));

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Fact]
        public void GivenAnOptionNullableIntFuncMatchReturnsExpectedNone()
        {
            var option = new Option<int?>(null);

            var result = option.Match(
                some: v => v,
                none: () => null);

            Assert.Equal(null, result);
        }

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
            public void FuncMatchReturnsExpectedSome()
            {
                var result = _option.Match(
                    some: v => v,
                    none: () => null);

                Assert.Equal(_foo, result);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => { someCalled = true; return 1; },
                    none: () => { noneCalled = true; return 1; });

                var someCalledButNoneNotCalled = someCalled && !noneCalled;
                Assert.True(someCalledButNoneNotCalled);
            }

            [Fact]
            public void ActionMatchWithNullSomeCaseThrowsException()
            {
                Action<Option<Foo>, Action<Foo>, Action> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, null, () => { }).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithNullNoneCaseThrowsException()
            {
                Action<Option<Foo>, Action<Foo>, Action> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, v => { }, null).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithBothCasesNullThrowsException()
            {
                Action<Option<Foo>, Action<Foo>, Action> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, null, null).AsThrowsDelegate());
            }

            public class Foo
            {
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
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: (Action)(() => noneCalled = true));

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }

            [Fact]
            public void FuncMatchReturnsExpectedNone()
            {
                var result = _option.Match(
                    some: v => v,
                    none: () => null);

                Assert.Equal(null, result);
            }

            [Fact]
            public void FuncMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => { someCalled = true; return 1; },
                    none: () => { noneCalled = true; return 1; });

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            public class Foo
            {
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
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _option.Match(
                    some: v => someCalled = true,
                    none: (Action)(() => noneCalled = true));

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }

            [Fact]
            public void ActionMatchWithNullSomeCaseThrowsException()
            {
                Action<Option<Foo>, Action<Foo>, Action> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, null, () => { }).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithNullNoneCaseThrowsException()
            {
                Action<Option<Foo>, Action<Foo>, Action> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, v => { }, null).AsThrowsDelegate());
            }

            [Fact]
            public void ActionMatchWithBothCasesNullThrowsException()
            {
                Action<Option<Foo>, Action<Foo>, Action> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, null, null).AsThrowsDelegate());
            }

            [Fact]
            public void FuncMatchReturnsExpectedNone()
            {
                var result = _option.Match(
                    some: v => v,
                    none: () => null);

                Assert.Equal(null, result);
            }

            public class Foo
            {
            }
        }
    }
}
