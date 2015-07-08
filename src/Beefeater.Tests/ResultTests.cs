using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests
{
    public class ResultTests
    {
        [Fact]
        public void ProvidedNullExceptionThrows()
        {
            Exception exception = null;

            Func<Exception, Result<string, Exception>> function = CreateResultFrom;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(function.AsActionUsing(exception).AsThrowsDelegate());
        }

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
            public void ConstructsSuccessfully()
            {
                Assert.NotNull(_result);
            }

            [Fact]
            public void IsSuccessful()
            {
                Assert.True(_result.Successful);
            }

            [Fact]
            public void ReturnsInputResult()
            {
                Assert.Equal(TestResult, _result.Value);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, InvalidOperationException>(_result, GetException);
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
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, e => false).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullNone()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullBoth()
            {
                Func<Func<string,bool>, Func<Exception,bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, null).AsThrowsDelegate());
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
            public void ConstructsSuccessfully()
            {
                Assert.NotNull(_result);
            }

            [Fact]
            public void IsSuccessful()
            {
                Assert.True(_result.Successful);
            }

            [Fact]
            public void ReturnsInputResult()
            {
                Assert.Equal(null, _result.Value);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, InvalidOperationException>(_result, GetException);
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
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, e => false).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullNone()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullBoth()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, null).AsThrowsDelegate());
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
            public void ConstructsSuccessfully()
            {
                Assert.NotNull(_result);
            }

            [Fact]
            public void IsNotSuccessful()
            {
                Assert.False(_result.Successful);
            }

            [Fact]
            public void ThrowsWhenYouAccessResult()
            {
                AssertThrowsException<string, InvalidOperationException>(_result, GetValue);
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
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, e => false).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullNone()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(v => true, null).AsThrowsDelegate());
            }

            [Fact]
            public void ThrowsWhenFuncMatchHasNullBoth()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<ArgumentNullException>(callFuncMatch.AsActionUsing(null, null).AsThrowsDelegate());
            }
        }

        public class WhenConstructedUsingDefault
        {
            private readonly Result<string, Exception> _result;

            public WhenConstructedUsingDefault()
            {
                _result = default(Result<string, Exception>);
            }

            [Fact]
            public void ThrowsWhenYouAccessSuccessful()
            {
                AssertThrowsException<bool, PanicException>(_result, GetSuccessful);
            }

            [Fact]
            public void ThrowsWhenYouAccessResult()
            {
                AssertThrowsException<string, PanicException>(_result, GetValue);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, PanicException>(_result, GetException);
            }

            [Fact]
            public void ThrowsWhenYouAccessFuncMatch()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<PanicException>(callFuncMatch.AsActionUsing(v => true, e => false).AsThrowsDelegate());
            }
        }

        public class WhenConstructedUsingDefaultConstructor
        {
            private readonly Result<string, Exception> _result;

            public WhenConstructedUsingDefaultConstructor()
            {
                _result = new Result<string, Exception>();
            }

            [Fact]
            public void ThrowsWhenYouAccessSuccessful()
            {
                AssertThrowsException<bool, PanicException>(_result, GetSuccessful);
            }

            [Fact]
            public void ThrowsWhenYouAccessResult()
            {
                AssertThrowsException<string, PanicException>(_result, GetValue);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, PanicException>(_result, GetException);
            }

            [Fact]
            public void ThrowsWhenYouAccessFuncMatch()
            {
                Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = _result.Match;
                Assert.Throws<PanicException>(callFuncMatch.AsActionUsing(v => true, e => false).AsThrowsDelegate());
            }
        }

        private static Result<string, Exception> CreateResultFrom(Exception exception)
        {
            return new Result<string, Exception>(exception);
        }

        private static void AssertThrowsException<T, TException>(Result<string, Exception> result, Func<Result<string, Exception>, T> action) where TException : Exception
        {
            Assert.Throws<TException>(action.AsActionUsing(result).AsThrowsDelegate());
        }

        private static Exception GetException(Result<string, Exception> result)
        {
            return result.Exception;
        }

        private static string GetValue(Result<string, Exception> result)
        {
            return result.Value;
        }

        private static bool GetSuccessful(Result<string, Exception> result)
        {
            return result.Successful;
        }
    }
}
