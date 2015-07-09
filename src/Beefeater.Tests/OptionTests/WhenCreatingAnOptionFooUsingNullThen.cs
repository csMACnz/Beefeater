using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
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
        public void ValueOrDefaultMatchesOriginalFoo()
        {
            Assert.Equal(null, _option.ValueOrDefault());
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

        [Fact]
        public void FuncMatchReturnsExpectedNone()
        {
            var result = _option.Match(
                some: v => v,
                none: () => null);

            Assert.Equal(null, result);
        }

        [Fact]
        public void ExplicitCastToAnFooThrowsException()
        {
            Assert.Throws<PanicException>(() => (Foo) _option);
        }

        public class Foo
        {
        }
    }
}