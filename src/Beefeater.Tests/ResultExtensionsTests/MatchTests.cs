﻿using System;
using System.Collections.Generic;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;
using Xunit.Extensions;

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
                bool noneCalled, someCalled;

                CallActionMatch(_result, out someCalled, out noneCalled);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                bool noneCalled, someCalled;

                CallFuncMatch(_result, out someCalled, out noneCalled);

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
                bool noneCalled, someCalled;

                CallActionMatch(_result, out someCalled, out noneCalled);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                bool noneCalled, someCalled;

                CallFuncMatch(_result, out someCalled, out noneCalled);

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
            public void FuncMatchReturnsExpectedNull()
            {
                var result = CallMatchToValue(_result);

                Assert.Equal(null, result);
            }

            [Fact]
            public void ActionMatchCallsNoneButNotSome()
            {
                bool noneCalled, someCalled;

                CallActionMatch(_result, out someCalled, out noneCalled);

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsNoneButNotSome()
            {
                bool noneCalled, someCalled;

                CallFuncMatch(_result, out someCalled, out noneCalled);

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessActionMatch()
        {
            var result = new Result<string, Exception>();

            Assert.Throws<PanicException>(_callActionMatch.AsActionUsing(result, v => { }, e => { }).AsThrowsDelegate());
        }

        [Fact]
        public void WhenConstructedUsingDefaultThrowsWhenYouAccessActionMatch()
        {
            var result = default(Result<string, Exception>);

            Assert.Throws<PanicException>(_callActionMatch.AsActionUsing(result, v => { }, e => { }).AsThrowsDelegate());
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessFuncMatch()
        {
            var result = new Result<string, Exception>();

            Assert.Throws<PanicException>(_callFuncMatch.AsActionUsing(result, v => true, e => false).AsThrowsDelegate());
        }

        [Fact]
        public void WhenConstructedUsingDefaultThrowsWhenYouAccessFuncMatch()
        {
            var result = default(Result<string, Exception>);

            Assert.Throws<PanicException>(_callFuncMatch.AsActionUsing(result, v => true, e => false).AsThrowsDelegate());
        }

        [Theory]
        [PropertyData("ThrowsTestData")]
        public void ThrowsWhenActionMatchHasNullSome(Result<string, Exception> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callActionMatch.AsActionUsing(result, null, e => { }).AsThrowsDelegate());
        }

        [Theory]
        [PropertyData("ThrowsTestData")]
        public void ThrowsWhenActionMatchHasNullNone(Result<string, Exception> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callActionMatch.AsActionUsing(result, v => { }, null).AsThrowsDelegate());
        }

        [Theory]
        [PropertyData("ThrowsTestData")]
        public void ThrowsWhenActionMatchHasNullBoth(Result<string, Exception> result)
        {

            Assert.Throws<ArgumentNullException>(
                _callActionMatch.AsActionUsing(result, null, null).AsThrowsDelegate());
        }

        [Theory]
        [PropertyData("ThrowsTestData")]
        public void ThrowsWhenFuncMatchHasNullSome(Result<string, Exception> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callFuncMatch.AsActionUsing(result, null, e => false).AsThrowsDelegate());
        }

        [Theory]
        [PropertyData("ThrowsTestData")]
        public void ThrowsWhenFuncMatchHasNullNone(Result<string, Exception> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callFuncMatch.AsActionUsing(result, v => true, null).AsThrowsDelegate());
        }

        [Theory]
        [PropertyData("ThrowsTestData")]
        public void ThrowsWhenFuncMatchHasNullBoth(Result<string, Exception> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callFuncMatch.AsActionUsing(result, null, null).AsThrowsDelegate());
        }

        private readonly Action<Result<string, Exception>, Action<string>, Action<Exception>> _callActionMatch =
            ResultExtensions.Match;
        private readonly Func<Result<string, Exception>, Func<string, bool>, Func<Exception, bool>, bool> _callFuncMatch =
            ResultExtensions.Match;

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
