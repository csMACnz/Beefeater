using System;
using BCLExtensions;
using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField
namespace Beefeater.Tests
{
    public class GeneralTests
    {
        [Fact]
        public void ProvidedValidExceptionReturns()
        {
            Exception exception = new Exception();

            Func<Exception, Result<string, Exception>> function = CreateResultFrom;

            function(exception);
        }

        [Fact]
        public void ProvidedNullExceptionThrows()
        {
            const Exception exception = null;

            Func<Exception, Result<string, Exception>> function = CreateResultFrom;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(function.AsActionUsing(exception));
        }

        [Fact]
        public void ResultOfStringStringCreatedWithOfValueIsCreatedCorrectly()
        {
            Result<string, string> result = Result<string, string>.OfValue("Happy");

            Assert.True(result.Successful && result.Value == "Happy");
        }

        [Fact]
        public void ResultOfStringStringCreatedWithOfErrorIsCreatedCorrectly()
        {
            Result<string, string> result = Result<string, string>.OfError("Error Message");

            Assert.True(!result.Successful && result.Error == "Error Message");
        }

        [Fact]
        public void ResultOfStringStringCreatedWithOfErrorOfNullThrows()
        {
            Func<string, Result<string, string>> ofError = Result<string, string>.OfError;
            Assert.Throws<ArgumentNullException>(ofError.AsActionUsing(null));
        }

        [Fact]
        public void ValidFooCanImplicityCastToResultOfFooString()
        {
            var foo = new Foo();

            Result<Foo, string> result = foo;

            Assert.Equal(foo, result.Value);
        }

        [Fact]
        public void StringCanImplicityCastToResultOfFooString()
        {
            const string error = "Hello";

            Result<Foo, string> result = error;

            Assert.Equal(error, result.Error);
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
            public void ConstructsSuccessfully()
            {
                Assert.NotNull(_result);
            }

            [Fact]
            public void IsSuccessful()
            {
                Assert.True(GetSuccessful(_result));
            }

            [Fact]
            public void ReturnsInputResult()
            {
                Assert.Equal(TestResult, _result.Value);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, InvalidOperationException>(_result, GetError);
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
            public void ConstructsSuccessfully()
            {
                Assert.NotNull(_result);
            }

            [Fact]
            public void IsSuccessful()
            {
                Assert.True(GetSuccessful(_result));
            }

            [Fact]
            public void ReturnsInputResult()
            {
                Assert.Equal(null, GetValue(_result));
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, InvalidOperationException>(_result, GetError);
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
            public void ConstructsSuccessfully()
            {
                Assert.NotNull(_result);
            }

            [Fact]
            public void IsNotSuccessful()
            {
                Assert.False(GetSuccessful(_result));
            }

            [Fact]
            public void WhenGetErrorThenGetsException()
            {
                var ex = GetError(_result);
                Assert.NotNull(ex);
            }

            [Fact]
            public void ThrowsWhenYouAccessResult()
            {
                AssertThrowsException<string, InvalidOperationException>(_result, GetValue);
            }
        }

        public class WhenConstructedUsingDefault
        {
            private readonly Result<string, Exception> _result;

            public WhenConstructedUsingDefault()
            {
                _result = default;
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
                AssertThrowsException<Exception, PanicException>(_result, GetError);
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
                AssertThrowsException<Exception, PanicException>(_result, GetError);
            }
        }

        private static Result<string, Exception> CreateResultFrom(Exception exception)
        {
            return Result<string, Exception>.OfError(exception);
        }

        private static void AssertThrowsException<T, TException>(Result<string, Exception> result, Func<Result<string, Exception>, T> action)
            where TException : Exception
        {
            Assert.Throws<TException>(action.AsActionUsing(result));
        }

        private static Exception GetError(Result<string, Exception> result)
        {
            return result.Error;
        }

        private static string GetValue(Result<string, Exception> result)
        {
            return result.Value;
        }

        private static bool GetSuccessful(Result<string, Exception> result)
        {
            return result.Successful;
        }

        private class Foo
        {
        }
    }
}
