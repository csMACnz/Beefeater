using System;
using System.Collections.Generic;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;

namespace Beefeater.Tests.EitherExtensionsTests
{
    public class MatchTests
    {
        public class ProvidedValidString
        {
            private const string TestResult = "My Result";
            private readonly Either<string, bool> _result;

            public ProvidedValidString()
            {
                const string result = TestResult;
                _result = Either<string, bool>.OfResult1(result);
            }

            [Fact]
            public void FuncMatchReturnsExpectedValue()
            {
                var result = CallMatchToResult1(_result);

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
            private readonly Either<string, bool> _result;

            public ProvidedNullString()
            {
                const string result = null;

                _result = Either<string, bool>.OfResult1(result);
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

        public class ProvidedBool
        {
            private readonly Either<string, bool> _result;

            public ProvidedBool()
            {
                _result = Either<string, bool>.OfResult2(true);
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
                var result = CallMatchToResult1(_result);

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
        public void WhenConstructedUsingDefaultConstructorWorksWithActionMatch()
        {
            var result = new Either<string, bool>();
            
            _callActionMatch.AsActionUsing(result, ActionHelpers.EmptyMethod, ActionHelpers.EmptyMethod);
        }

        [Fact]
        public void WhenConstructedUsingDefaultWorksWithActionMatch()
        {
            var result = default(Either<string, bool>);

            _callActionMatch.AsActionUsing(result, ActionHelpers.EmptyMethod, ActionHelpers.EmptyMethod);
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorWorksWithFuncMatch()
        {
            var result = new Either<string, bool>();

            _callFuncMatch.AsActionUsing(result, FuncHelpers.ReturnTrue, FuncHelpers.ReturnFalse);
        }

        [Fact]
        public void WhenConstructedUsingDefaultWorksWithFuncMatch()
        {
            var result = default(Either<string, bool>);

            _callFuncMatch.AsActionUsing(result, FuncHelpers.ReturnTrue, FuncHelpers.ReturnFalse);
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenActionMatchHasNullSome(Either<string, bool> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callActionMatch.AsActionUsing(result, null, ActionHelpers.EmptyMethod));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenActionMatchHasNullNone(Either<string, bool> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callActionMatch.AsActionUsing(result, ActionHelpers.EmptyMethod, null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenActionMatchHasNullBoth(Either<string, bool> result)
        {

            Assert.Throws<ArgumentNullException>(
                _callActionMatch.AsActionUsing(result, null, null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenFuncMatchHasNullSome(Either<string, bool> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callFuncMatch.AsActionUsing(result, null, FuncHelpers.ReturnFalse));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenFuncMatchHasNullNone(Either<string, bool> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callFuncMatch.AsActionUsing(result, FuncHelpers.ReturnTrue, null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenFuncMatchHasNullBoth(Either<string, bool> result)
        {
            Assert.Throws<ArgumentNullException>(
                _callFuncMatch.AsActionUsing(result, null, null));
        }

        private readonly Action<Either<string, bool>, Action<string>, Action<bool>> _callActionMatch =
            EitherExtensions.Match;
        private readonly Func<Either<string, bool>, Func<string, bool>, Func<bool, bool>, bool> _callFuncMatch =
            EitherExtensions.Match;

        public static IEnumerable<object[]> ThrowsTestData
        {
            get
            {
                yield return new object[] { Either<string, bool>.OfResult1("My Result") };
                yield return new object[] { Either<string, bool>.OfResult1("") };
                yield return new object[] { Either<string, bool>.OfResult1(null) };
                yield return new object[] { Either<string, bool>.OfResult2(true) };
                yield return new object[] { Either<string, bool>.OfResult2(false) };
            }
        }

        private static void CallFuncMatch<TResult1, TResult2>(Either<TResult1, TResult2> result, out bool processItem1Called, out bool processItem2Called)
        {
            var processItem1 = false;
            var processItem2 = false;

            result.Match(
                processItem1: v =>
                {
                    processItem1 = true;
                    return 1;
                },
                processItem2: e =>
                {
                    processItem2 = true;
                    return 1;
                });

            processItem1Called = processItem1;
            processItem2Called = processItem2;
        }

        private static void CallActionMatch<TResult1, TResult2>(Either<TResult1, TResult2> result, out bool processItem1Called, out bool processItem2Called)
        {
            var processItem1 = false;
            var processItem2 = false;

            result.Match(
                processItem1: (Action<TResult1>)(v => processItem1 = true),
                processItem2: (Action<TResult2>)(e => processItem2 = true));

            processItem1Called = processItem1;
            processItem2Called = processItem2;
        }

        private static TResult1 CallMatchToResult1<TResult1, TResult2>(Either<TResult1, TResult2> result) where TResult1 : class
        {
            var value = result.Match(
                processItem1: v => v,
                processItem2: e => null);
            return value;
        }

        private static bool CallMatchToBool<TResult1, TResult2>(Either<TResult1, TResult2> result)
        {
            var value = result.Match(
                processItem1: v => true,
                processItem2: e => false);
            return value;
        }
    }
}
