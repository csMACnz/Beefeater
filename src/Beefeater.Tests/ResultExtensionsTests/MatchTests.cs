using System;
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
            public void ActionMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: (Action<string>) (v => someCalled = true),
                    none: err => noneCalled = true);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: v =>
                    {
                        someCalled = true;
                        return 1;
                    },
                    none: err =>
                    {
                        noneCalled = true;
                        return 1;
                    });

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
            public void ActionMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: (Action<string>) (v => someCalled = true),
                    none: err => noneCalled = true);

                var someNotCalledAndNoneCalled = someCalled && !noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsSomeButNotNone()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: v =>
                    {
                        someCalled = true;
                        return 1;
                    },
                    none: err =>
                    {
                        noneCalled = true;
                        return 1;
                    });

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
            public void ActionMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: (Action<string>) (v => someCalled = true),
                    none: err => noneCalled = true);

                var someNotCalledAndNoneCalled = !someCalled && noneCalled;
                Assert.True(someNotCalledAndNoneCalled);
            }

            [Fact]
            public void FuncMatchCallsNoneButNotSome()
            {
                var noneCalled = false;
                var someCalled = false;

                _result.Match(
                    some: v =>
                    {
                        someCalled = true;
                        return 1;
                    },
                    none: err =>
                    {
                        noneCalled = true;
                        return 1;
                    });

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
                yield return new object[] { new Result<string, Exception>("My Result") };
                yield return new object[] { new Result<string, Exception>("") };
                yield return new object[] {new Result<string, Exception>((string)null)};
                yield return new object[] {new Result<string, Exception>(new Exception())};
                yield return new object[] {new Result<string, Exception>(new PanicException())};
            }
        }
    }
}
