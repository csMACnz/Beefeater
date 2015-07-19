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
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void GivenAnOptionOfNullableIntActionMatchCallsSomeButNotNone(int value)
        {
            var option = new Option<int?>(value);

            bool someCalled, noneCalled;
            CallActionMatch(option, out someCalled, out noneCalled);

            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void GivenAnOptionOfNullableIntActionMatchCallsNoneButNotSome(int value)
        {
            var option = new Option<int?>(value);

            bool someCalled, noneCalled;
            CallActionMatch(option, out someCalled, out noneCalled);

            var someCalledButNoneNotCalled = someCalled && !noneCalled;
            Assert.True(someCalledButNoneNotCalled);
        }

        [Fact]
        public void GivenANoneOfOptionIntActionMatchCallsNoneButNotSome()
        {
            bool noneCalled, someCalled;
            var option = Option<int>.None;

            CallActionMatch(option, out someCalled, out noneCalled);

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Fact]
        public void ActionMatchCallsNoneButNotSome()
        {
            var option = Option<int?>.None;
            bool noneCalled, someCalled;

            CallActionMatch(option, out someCalled, out noneCalled);

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Fact]
        public void GivenAnOptionNullableIntWhenNullActionMatchCallsNoneButNotSome()
        {
            var option = new Option<int?>(null);
            bool noneCalled, someCalled;

            CallActionMatch(option, out someCalled, out noneCalled);

            var someNotCalledButNoneCalled = !someCalled && noneCalled;
            Assert.True(someNotCalledButNoneCalled);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void GivenAnOptionOfIntFuncMatchReturnsExpectedSome(int value)
        {
            var option = new Option<int>(value);

            var result = CallMatchToInt(option);

            Assert.Equal(value, result);
        }

        [Fact]
        public void GivenANoneOfOptionIntFuncMatchReturnsExpectedNone()
        {
            var option = Option<int>.None;

            var result = CallMatchToInt(option);

            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void GivenAnOptionOfIntFuncMatchReturnsNullableExpectedSome(int value)
        {
            var option = new Option<int>(value);

            var result = CallMatchToNullableInt(option);

            Assert.Equal(value, result);
        }

        [Fact]
        public void GivenANoneOfOptionIntFuncMatchReturnsNullableExpectedNone()
        {
            var option = Option<int>.None;

            var result = CallMatchToNullableInt(option);

            Assert.Equal(null, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void GivenAnOptionOfNullableIntFuncMatchReturnsExpectedSome(int value)
        {
            var option = new Option<int?>(value);

            var result = CallMatchNullableToInt(option);

            Assert.Equal(value, result);
        }

        [Fact]
        public void GivenANoneOptionNullableIntFuncMatchReturnsExpectedNone()
        {
            var option = Option<int?>.None;

            var result = CallMatchNullableToInt(option);

            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-12)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void GivenAnOptionOfNullableIntFuncMatchReturnsExpectedSomeAsNullable(int value)
        {
            var option = new Option<int?>(value);

            var result = CallMatchNullableToNullable(option);

            Assert.Equal(value, result);
        }

        [Fact]
        public void GivenAnOptionNullableIntFuncMatchReturnsExpectedNone()
        {
            var option = new Option<int?>(null);

            var result = CallMatchNullableToNullable(option);

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
                var result = CallMatchTo(_option);

                Assert.Equal(_foo, result);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                bool noneCalled;
                bool someCalled;
                CallFuncMatch(_option, out someCalled, out noneCalled);

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
                bool noneCalled, someCalled;

                CallActionMatch(_option, out someCalled, out noneCalled);

                var someNotCalledButNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledButNoneCalled);
            }

            [Fact]
            public void FuncMatchReturnsExpectedNone()
            {
                var result = CallMatchTo(_option);

                Assert.Equal(null, result);
            }

            [Fact]
            public void FuncMatchCallsNoneButNotSome()
            {
                bool noneCalled, someCalled;

                CallFuncMatch(_option, out someCalled, out noneCalled);

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
                bool noneCalled, someCalled;

                CallActionMatch(_option, out someCalled, out noneCalled);

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
                var result = CallMatchTo(_option);

                Assert.Equal(null, result);
            }

            [Fact]
            public void FuncMatchWithNullSomeCaseThrowsException()
            {
                Func<Option<Foo>, Func<Foo, bool>, Func<bool>, bool> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, null, () => true ).AsThrowsDelegate());
            }

            [Fact]
            public void FuncMatchWithNullNoneCaseThrowsException()
            {
                Func<Option<Foo>, Func<Foo, bool>, Func<bool>, bool> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void FuncMatchWithBothCasesNullThrowsException()
            {
                Func<Option<Foo>, Func<Foo, bool>, Func<bool>, bool> callActionMatch = OptionExtensions.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(_option, null, null).AsThrowsDelegate());
            }

            public class Foo
            {
            }
        }

        private static void CallFuncMatch<T>(Option<T> option, out bool someCalled, out bool noneCalled)
        {
            var none = false;
            var some = false;

            option.Match(
                some: v =>
                {
                    some = true;
                    return 1;
                },
                none: () =>
                {
                    none = true;
                    return 1;
                });

            someCalled = some;
            noneCalled = none;
        }

        private static void CallActionMatch<T>(Option<T> option, out bool someCalled, out bool noneCalled)
        {
            var none = false;
            var some = false;

            option.Match(
                some: v => some = true,
                none: (Action)(() => none = true));

            noneCalled = none;
            someCalled = some;
        }

        private static T CallMatchTo<T>(Option<T> option) where T : class
        {
            var result = option.Match(
                some: v => v,
                none: () => null);
            return result;
        }

        private static int? CallMatchNullableToNullable(Option<int?> option)
        {
            var result = option.Match(
                some: v => v,
                none: () => null);
            return result;
        }

        private static int CallMatchNullableToInt(Option<int?> option)
        {
            var result = option.Match(
                // ReSharper disable once PossibleInvalidOperationException
                some: v => v.Value,
                none: () => -1);
            return result;
        }

        private static int CallMatchToInt(Option<int> option)
        {
            var result = option.Match(
                some: v => v,
                none: () => -1);
            return result;
        }

        private static int? CallMatchToNullableInt(Option<int> option)
        {
            var result = option.Match<int, int?>(
                some: v => v,
                none: () => null);
            return result;
        }

    }
}
