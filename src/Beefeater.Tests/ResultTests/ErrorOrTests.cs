using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCLExtensions;
using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField
namespace Beefeater.Tests.ResultTests
{
    public class ErrorOrTests
    {
        private const string ReplacedString = "REPLACED";

        private static Result<string, Exception> MapToReplaceString(string arg)
        {
            return ReplacedString;
        }

        private static Result<bool, Exception> MapToBoolean(string arg)
        {
            return true;
        }

        private static Task<Result<string, Exception>> MapToReplaceStringAsync(string arg)
        {
            return Task.FromResult<Result<string, Exception>>(ReplacedString);
        }

        private static Task<Result<bool, Exception>> MapToBooleanAsync(string arg)
        {
            return Task.FromResult<Result<bool, Exception>>(true);
        }

        private static void ErrorOrFunctionIsCalled<TValue, TError>(Result<TValue, TError> result, out bool someCalled)
        {
            var some = false;

            result.ErrorOr(some: v =>
            {
                some = true;
                return v;
            });

            someCalled = some;
        }

        private static async Task<bool> ErrorOrAsyncFunctionIsCalled<TValue, TError>(Result<TValue, TError> result)
        {
            var some = false;

            await result.ErrorOrAsync(some: v =>
            {
                some = true;
                return Task.FromResult<Result<TValue, TError>>(v);
            });

            return some;
        }

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
            public void ErrorOrReturnsCorrectlyReplacedValue()
            {
                var result = _result.ErrorOr(MapToReplaceString);

                Assert.True(result.Successful);
                Assert.Equal(ReplacedString, result.Value);
            }

            [Fact]
            public async Task ErrorOrAsyncReturnsCorrectlyReplacedValue()
            {
                var result = await _result.ErrorOrAsync(MapToReplaceStringAsync);

                Assert.True(result.Successful);
                Assert.Equal(ReplacedString, result.Value);
            }

            [Fact]
            public void ErrorOrReturnsCorrectlyReplacedType()
            {
                var result = _result.ErrorOr(MapToBoolean);

                Assert.True(result.Successful);
                Assert.True(result.Value);
            }

            [Fact]
            public async Task ErrorOrAsyncReturnsCorrectlyReplacedType()
            {
                var result = await _result.ErrorOrAsync(MapToBooleanAsync);

                Assert.True(result.Successful);
                Assert.True(result.Value);
            }

            [Fact]
            public void ErrorOrCorrectlyCallsFunction()
            {
                ErrorOrFunctionIsCalled(_result, out var wasCalled);

                Assert.True(wasCalled);
            }

            [Fact]
            public async Task ErrorOrAsyncCorrectlyCallsFunction()
            {
                var wasCalled = await ErrorOrAsyncFunctionIsCalled(_result);

                Assert.True(wasCalled);
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
            public void ErrorOrReturnsCorrectlyReplacedValue()
            {
                var result = _result.ErrorOr(MapToReplaceString);

                Assert.True(result.Successful);
                Assert.Equal(ReplacedString, result.Value);
            }

            [Fact]
            public async Task ErrorOrAsyncReturnsCorrectlyReplacedValue()
            {
                var result = await _result.ErrorOrAsync(MapToReplaceStringAsync);

                Assert.True(result.Successful);
                Assert.Equal(ReplacedString, result.Value);
            }

            [Fact]
            public void ErrorOrReturnsCorrectlyReplacedType()
            {
                var result = _result.ErrorOr(MapToBoolean);

                Assert.True(result.Successful);
                Assert.True(result.Value);
            }

            [Fact]
            public async Task ErrorOrAsyncReturnsCorrectlyReplacedType()
            {
                var result = await _result.ErrorOrAsync(MapToBooleanAsync);

                Assert.True(result.Successful);
                Assert.True(result.Value);
            }

            [Fact]
            public void ErrorOrCorrectlyCallsFunction()
            {
                ErrorOrFunctionIsCalled(_result, out var wasCalled);

                Assert.True(wasCalled);
            }

            [Fact]
            public async Task ErrorOrAsyncCorrectlyCallsFunction()
            {
                var wasCalled = await ErrorOrAsyncFunctionIsCalled(_result);

                Assert.True(wasCalled);
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
            public void ErrorOrReturnsCorrectlyReplacedValue()
            {
                var result = _result.ErrorOr(MapToReplaceString);

                Assert.False(result.Successful);
                Assert.IsType<Exception>(result.Error);
            }

            [Fact]
            public async Task ErrorOrAsyncReturnsCorrectlyReplacedValue()
            {
                var result = await _result.ErrorOrAsync(MapToReplaceStringAsync);

                Assert.False(result.Successful);
                Assert.IsType<Exception>(result.Error);
            }

            [Fact]
            public void ErrorOrReturnsCorrectlyReplacedType()
            {
                var result = _result.ErrorOr(MapToBoolean);

                Assert.False(result.Successful);
                Assert.IsType<Exception>(result.Error);
            }

            [Fact]
            public async Task ErrorOrAsyncReturnsCorrectlyReplacedType()
            {
                var result = await _result.ErrorOrAsync(MapToBooleanAsync);

                Assert.False(result.Successful);
                Assert.IsType<Exception>(result.Error);
            }

            [Fact]
            public void ErrorOrCorrectlyCallsFunction()
            {
                ErrorOrFunctionIsCalled(_result, out var wasCalled);

                Assert.False(wasCalled);
            }

            [Fact]
            public async Task ErrorOrAsyncCorrectlyCallsFunction()
            {
                var wasCalled = await ErrorOrAsyncFunctionIsCalled(_result);

                Assert.False(wasCalled);
            }
        }

        [Fact]
        public void WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessErrorOr()
        {
            var result = new Result<string, Exception>();

            Func<Func<string, Result<string, Exception>>, Result<string, Exception>> callErrorOr = result.ErrorOr;
            Assert.Throws<PanicException>(callErrorOr.AsActionUsing(MapToReplaceString));
        }

        [Fact]
        public void WhenConstructedUsingDefaultThrowsWhenYouAccessErrorOr()
        {
            var result = default(Result<string, Exception>);

            Func<Func<string, Result<string, Exception>>, Result<string, Exception>> callErrorOr = result.ErrorOr;
            Assert.Throws<PanicException>(callErrorOr.AsActionUsing(MapToReplaceString));
        }

        [Fact]
        public async Task WhenConstructedUsingDefaultConstructorThrowsWhenYouAccessErrorOrAsync()
        {
            var result = new Result<string, Exception>();

            Func<Func<string, Task<Result<string, Exception>>>, Task<Result<string, Exception>>> callErrorOrAsync = result.ErrorOrAsync;
            await Assert.ThrowsAsync<PanicException>(AsAsyncUsing(callErrorOrAsync, MapToReplaceStringAsync));
        }

        [Fact]
        public async Task WhenConstructedUsingDefaultThrowsWhenYouAccessErrorOrAsync()
        {
            var result = default(Result<string, Exception>);

            Func<Func<string, Task<Result<string, Exception>>>, Task<Result<string, Exception>>> callErrorOrAsync = result.ErrorOrAsync;
            await Assert.ThrowsAsync<PanicException>(AsAsyncUsing(callErrorOrAsync, MapToReplaceStringAsync));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public void ThrowsWhenErrorOrHasNullSomeFunc(Result<string, Exception> result)
        {
            Func<Func<string, Result<string, Exception>>, Result<string, Exception>> callErrorOr = result.ErrorOr;
            Assert.Throws<ArgumentNullException>(callErrorOr.AsActionUsing(null));
        }

        [Theory]
        [MemberData(nameof(ThrowsTestData))]
        public async Task ThrowsWhenErrorOrAsyncHasNullSomeFunc(Result<string, Exception> result)
        {
            Func<Func<string, Task<Result<string, Exception>>>, Task<Result<string, Exception>>> callErrorOrAsync = result.ErrorOrAsync;
            await Assert.ThrowsAsync<ArgumentNullException>(AsAsyncUsing(callErrorOrAsync, null));
        }

        private static Func<Task> AsAsyncUsing<T1, TResult>(Func<T1, Task<TResult>> asyncTask, T1 arg)
        {
#pragma warning disable CC0031 // Check for null before calling a delegate
            return async () => await asyncTask(arg);
#pragma warning restore CC0031 // Check for null before calling a delegate
        }

        public static IEnumerable<object[]> ThrowsTestData
        {
            get
            {
                yield return new object[] { Result<string, Exception>.OfValue("My Result") };
                yield return new object[] { Result<string, Exception>.OfValue(string.Empty) };
                yield return new object[] { Result<string, Exception>.OfValue(null) };
                yield return new object[] { Result<string, Exception>.OfError(new Exception()) };
                yield return new object[] { Result<string, Exception>.OfError(new PanicException()) };
            }
        }
    }
}