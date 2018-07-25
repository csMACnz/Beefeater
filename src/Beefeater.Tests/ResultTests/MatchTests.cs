using System;
using System.Collections.Generic;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;

namespace Beefeater.Tests.ResultTests
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
                _result = Result<string, Exception>.OfValue(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedValue()
            {
                var result = CallMatchToValue(_result);

                Assert.Equal(TestResult, result);
            }

            [Fact]
            public void ActionMatchCallsSomeButNotNone()
            {
                CallActionMatch(_result, out bool someCalled, out bool noneCalled);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                CallFuncMatch(_result, out bool someCalled, out bool noneCalled);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }
        }

        public class ProvidedNullString
        {
            private readonly Result<string, Exception> _result;

            public ProvidedNullString()
            {
                const string result = null;

                _result = Result<string, Exception>.OfValue(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedValue()
            {
                var result = CallMatchToBool(_result);

                Assert.True(result);
            }

            [Fact]
            public void ActionMatchCallsSomeButNotNone()
            {
                CallActionMatch(_result, out bool someCalled, out bool noneCalled);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {

                CallFuncMatch(_result, out bool someCalled, out bool noneCalled);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }
        }

        public class ProvidedException
        {
            private readonly Result<string, Exception> _result;

            public ProvidedException()
            {
                Exception result = new Exception();
                _result = Result<string, Exception>.OfError(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedValue()
            {
                var result = CallMatchToBool(_result);

                Assert.False(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedNull()
            {
                var result = CallMatchToValue(_result);

                Assert.Equal(null, result);
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                CallActionMatch(_result, out bool someCalled, out bool noneCalled);

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsNoneButNotSome()
            {
                CallFuncMatch(_result, out bool someCalled, out bool noneCalled);

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessActionMatch()
        {
            var result = new Result<string, Exception>();

            Action<Action<string>, Action<Exception>> callActionMatch = result.Match;
            Assert.Throws<PanicException>(callActionMatch.AsActionUsing(ActionHelpers.EmptyMethod, ActionHelpers.EmptyMethod));
        }

        [Fact]
        public void WhenConstructedUsingDefaultThrowsWhenYouAccessActionMatch()
        {
            var result = default(Result<string, Exception>);

            Action<Action<string>, Action<Exception>> callActionMatch = result.Match;
            Assert.Throws<PanicException>(callActionMatch.AsActionUsing(ActionHelpers.EmptyMethod, ActionHelpers.EmptyMethod));
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessFuncMatch()
        {
            var result = new Result<string, Exception>();

            Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = result.Match;
            Assert.Throws<PanicException>(callFuncMatch.AsActionUsing(FuncHelpers.ReturnTrue, FuncHelpers.ReturnFalse));
        }

        [Fact]
        public void WhenConstructedUsingDefaultThrowsWhenYouAccessFuncMatch()
        {
            var result = default(Result<string, Exception>);

            Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = result.Match;
            Assert.Throws<PanicException>(callFuncMatch.AsActionUsing(FuncHelpers.ReturnTrue, FuncHelpers.ReturnFalse));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenActionMatchHasNullSome(Result<string, Exception> result)
        {
            Action<Action<string>, Action<Exception>> callActionMatch = result.Match;
            Assert.Throws<ArgumentNullException>(
                callActionMatch.AsActionUsing(null, ActionHelpers.EmptyMethod));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenActionMatchHasNullNone(Result<string, Exception> result)
        {
            Action<Action<string>, Action<Exception>> callActionMatch = result.Match;
            Assert.Throws<ArgumentNullException>(
                callActionMatch.AsActionUsing(ActionHelpers.EmptyMethod, null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenActionMatchHasNullBoth(Result<string, Exception> result)
        {
            Action<Action<string>, Action<Exception>> callActionMatch = result.Match;
            Assert.Throws<ArgumentNullException>(
                callActionMatch.AsActionUsing(null, null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenFuncMatchHasNullSome(Result<string, Exception> result)
        {
            Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = result.Match;
            Assert.Throws<ArgumentNullException>(
                callFuncMatch.AsActionUsing(null, FuncHelpers.ReturnFalse));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenFuncMatchHasNullNone(Result<string, Exception> result)
        {
            Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = result.Match;
            Assert.Throws<ArgumentNullException>(
                callFuncMatch.AsActionUsing(FuncHelpers.ReturnTrue, null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenFuncMatchHasNullBoth(Result<string, Exception> result)
        {
            Func<Func<string, bool>, Func<Exception, bool>, bool> callFuncMatch = result.Match;
            Assert.Throws<ArgumentNullException>(
                callFuncMatch.AsActionUsing(null, null));
        }

        public static IEnumerable<object[]> ThrowsTestData
        {
            get
            {
                yield return new object[] {Result<string, Exception>.OfValue("My Result")};
                yield return new object[] {Result<string, Exception>.OfValue("")};
                yield return new object[] {Result<string, Exception>.OfValue(null)};
                yield return new object[] {Result<string, Exception>.OfError(new Exception())};
                yield return new object[] {Result<string, Exception>.OfError(new PanicException())};
            }
        }

        private static void CallFuncMatch<TValue, TError>(Result<TValue, TError> result, out bool someCalled, out bool noneCalled)
        {
            var none = false;
            var some = false;

            result.Match(
                some: v =>
                {
                    some = true;
                    return 1;
                },
                none: e =>
                {
                    none = true;
                    return 1;
                });

            someCalled = some;
            noneCalled = none;
        }

        private static void CallActionMatch<TValue, TError>(Result<TValue, TError> result, out bool someCalled, out bool noneCalled)
        {
            var none = false;
            var some = false;

            result.Match(
                some: v => some = true,
                none: (Action<TError>)(e => none = true));

            noneCalled = none;
            someCalled = some;
        }

        private static TValue CallMatchToValue<TValue, TError>(Result<TValue, TError> result) where TValue : class
        {
            var value = result.Match(
                some: v => v,
                none: e => null);
            return value;
        }

        private static bool CallMatchToBool<TValue, TError>(Result<TValue, TError> result)
        {
            var value = result.Match(
                some: v => true,
                none: e => false);
            return value;
        }
    }
}
