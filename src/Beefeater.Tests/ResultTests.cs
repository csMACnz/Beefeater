using System;
using BCLExtensions;
using Beefeater.Tests.TestHelpers;
using Xunit;

namespace Beefeater.Tests
{
    public class ResultTests
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
                AssertThrowsException<string, InvalidOperationException>(_result, GetResult);
            }
        }

        [Fact]
        public void ProvidedNullExceptionThrows()
        {
            Exception exception = null;

            Func<Exception, Result<string, Exception>> function = CreateResultFrom;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(function.AsActionUsing(exception).AsThrowsDelegate());
        }

        private static Result<string, Exception> CreateResultFrom(Exception exception)
        {
            return new Result<string, Exception>(exception);
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
                AssertThrowsException<string, PanicException>(_result, GetResult);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, PanicException>(_result, GetException);
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
                AssertThrowsException<string, PanicException>(_result, GetResult);
            }

            [Fact]
            public void ThrowsWhenYouAccessException()
            {
                AssertThrowsException<Exception, PanicException>(_result, GetException);
            }
        }


        private static void AssertThrowsException<T, TException>(Result<string, Exception> result, Func<Result<string, Exception>, T> action) where TException : Exception
        {
            Assert.Throws<TException>(action.AsActionUsing(result).AsThrowsDelegate());
        }

        private static Exception GetException(Result<string, Exception> result)
        {
            return result.Exception;
        }

        private static string GetResult(Result<string, Exception> result)
        {
            return result.Value;
        }

        private static bool GetSuccessful(Result<string, Exception> result)
        {
            return result.Successful;
        }
    }
}
