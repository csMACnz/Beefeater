using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.ResultExtensionsTests
{
    public class MatchTests
    {

        public class ProvidedValidString
        {
            private const string TestResult = "My Result";
            private readonly Result<string, Exception> _result;

            public ProvidedValidString()
            {
                const string result = TestResult;
                _result = new Result<string, Exception>(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedValue()
            {
                var result = _result.Match(
                    some: v => v,
                    none: err => null);

                Assert.Equal(TestResult, result);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: v => { someCalled = true; return 1; },
                    none: err => { noneCalled = true; return 1; });

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullSome()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, null, e => false).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullNone()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullBoth()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, null, null).AsThrowsDelegate());
            }
        }

        public class ProvidedNullString
        {
            private readonly Result<string, Exception> _result;

            public ProvidedNullString()
            {
                const string result = null;

                _result = new Result<string, Exception>(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedValue()
            {
                var result = _result.Match(
                    some: v => true,
                    none: err => false);

                Assert.True(result);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: v => { someCalled = true; return 1; },
                    none: err => { noneCalled = true; return 1; });

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullSome()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, null, e => false).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullNone()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullBoth()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, null, null).AsThrowsDelegate());
            }
        }

        public class ProvidedException
        {
            private readonly Result<string, Exception> _result;

            public ProvidedException()
            {
                Exception result = new Exception();
                _result = new Result<string, Exception>(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedNull()
            {
                var result = _result.Match(
                    some: v => v,
                    none: err => null);

                Assert.Equal(null, result);
            }

            [Fact]
            public void FuncMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: v => { someCalled = true; return 1; },
                    none: err => { noneCalled = true; return 1; });

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullSome()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, null, e => false).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullNone()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullBoth()
            {
                Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(_result, null, null).AsThrowsDelegate());
            }
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessFuncMatch()
        {
            var result = new Result<string, Exception>();

            Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
            Assert.Throws<PanicException>(callFuncMatch.AsActionUsing(result, v => true, e => false).AsThrowsDelegate());
        }
        [Fact]
        public void WhenConstructedUsingDefaultThrowsWhenYouAccessFuncMatch()
        {
            var result = default(Result<string, Exception>);

            Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = ResultExtensions.Match;
            Assert.Throws<PanicException>(callFuncMatch.AsActionUsing(result, v => true, e => false).AsThrowsDelegate());
        }
    }
}
