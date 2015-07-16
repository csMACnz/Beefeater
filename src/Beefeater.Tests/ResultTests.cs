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

        [Fact]
        public void ResultOfStringStringCreatedWithOfValueIsCreatedCorrectly()
        {
            Result<string,string> result = Result<string, string>.OfValue("Happy");

            Assert.True(result.Successful && result.Value == "Happy");
        }

        [Fact]
        public void ResultOfStringStringCreatedWithOfErrorIsCreatedCorrectly()
        {
            Result<string,string> result = Result<string, string>.OfError("Error Message");

            Assert.True(!result.Successful && result.Error == "Error Message");
        }

        [Fact]
        public void ResultOfStringStringCreatedWithOfErrorOfNullThrows()
        {
            Func<string, Result<string, string>> ofError = Result<string, string>.OfError;
            Assert.Throws<ArgumentNullException>(ofError.AsActionUsing(null).AsThrowsDelegate());
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
                Assert.False(_result.Successful);
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

        private static void AssertThrowsException<T, TException>(Result<string, Exception> result, Func<Result<string, Exception>, T> action) where TException : Exception
        {
            Assert.Throws<TException>(action.AsActionUsing(result).AsThrowsDelegate());
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
    }
}
