using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
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
            public void ValueOrDefaultMatchesOriginalFoo()
            {
                Assert.Equal(_foo, _option.ValueOrDefault());
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
                    some: v => { someCalled = true;return 1; },
                    none: () => { noneCalled = true; return 1; });

                var someCalledButNoneNotCalled = someCalled && !noneCalled;
                Assert.True(someCalledButNoneNotCalled);
            }

            [Fact]
            public void FuncMatchWithNullSomeCaseThrowsException()
            {
                Func<Func<Foo, bool>, Func<bool>, bool> callFuncMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, () => false).AsThrowsDelegate());
            }

            [Fact]
            public void FuncMatchWithNullNoneCaseThrowsException()
            {
                Func<Func<Foo, bool>, Func<bool>, bool> callFuncMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void FuncMatchWithBothCasesNullThrowsException()
            {
                Func<Func<Foo, bool>, Func<bool>, bool> callActionMatch = _option.Match;
                Assert.Throws<ArgumentNullException>(callActionMatch.AsActionUsing(null, null).AsThrowsDelegate());
            }

            public class Foo
            {
            }
        }
}
