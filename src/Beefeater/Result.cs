using System;
using System.Collections.Generic;

namespace Beefeater
{
    /// <summary>
    /// Represents a result that is either successful with a value or has an error.
    /// </summary>
    /// <typeparam name="TResult">The result, on success.</typeparam>
    /// <typeparam name="TError">The Error, on failure.</typeparam>
    public struct Result<TResult, TError> : IEquatable<Result<TResult, TError>>
    {
        public static implicit operator Result<TResult, TError>(TResult value)
        {
            return new Result<TResult, TError>(value);
        }

        public static implicit operator Result<TResult, TError>(TError error)
        {
            return new Result<TResult, TError>(error);
        }

        public static bool operator ==(Result<TResult, TError> result1, Result<TResult, TError> result2)
        {
            return result1.Equals(result2);
        }

        public static bool operator !=(Result<TResult, TError> result1, Result<TResult, TError> result2)
        {
            return !(result1 == result2);
        }

        private readonly TError _error;
        private readonly TResult _result;
        private readonly bool _successful;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TResult, TError}"/> struct.
        /// </summary>
        /// <param name="result">The result on success.</param>
        private Result(TResult result)
        {
            _successful = true;
            _result = result;
            _error = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TResult, TError}"/> struct.
        /// </summary>
        /// <param name="error">The error on failure.</param>
        private Result(TError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            _successful = false;
            _error = error;
            _result = default;
        }

        /// <summary>
        /// Gets the Error.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Error cannot be accessed on successful Result.</exception>
        public TError Error
        {
            get
            {
                if (_successful)
                {
                    throw new InvalidOperationException("Error cannot be accessed on successful Result.");
                }

                if (_error != null)
                {
                    return _error;
                }

                throw new PanicException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Result{TResult, TError}"/> is successful.
        /// </summary>
        public bool Successful
        {
            get
            {
                if (!_successful && _error == null)
                {
                    throw new PanicException();
                }

                return _successful;
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Result cannot be accessed on unsuccessful Result.</exception>
        public TResult Value
        {
            get
            {
                if (_successful)
                {
                    return _result;
                }

                if (_error == null)
                {
                    throw new PanicException();
                }

                throw new InvalidOperationException("Result cannot be accessed on unsuccessful Result.");
            }
        }

        public static Result<TResult, TError> OfError(TError error)
        {
            return new Result<TResult, TError>(error);
        }

        public static Result<TResult, TError> OfValue(TResult value)
        {
            return new Result<TResult, TError>(value);
        }

        public void Match(
            Action<TResult> some,
            Action<TError> none)
        {
            if (some == null)
            {
                throw new ArgumentNullException(nameof(some));
            }

            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            if (Successful)
            {
                some(Value);
            }
            else
            {
                none(Error);
            }
        }

        public TValue Match<TValue>(
            Func<TResult, TValue> some,
            Func<TError, TValue> none)
        {
            if (some == null)
            {
                throw new ArgumentNullException(nameof(some));
            }

            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            return Successful ? some(Value) : none(Error);
        }

        public override bool Equals(object obj)
        {
            return obj is Result<TResult, TError> && Equals((Result<TResult, TError>)obj);
        }

        public bool Equals(Result<TResult, TError> other)
        {
            return EqualityComparer<TError>.Default.Equals(_error, other._error) &&
                   EqualityComparer<TResult>.Default.Equals(_result, other._result) &&
                   _successful == other._successful;
        }

        public override int GetHashCode()
        {
            var hashCode = -183572109;
            hashCode = (hashCode * -1521134295) + EqualityComparer<TError>.Default.GetHashCode(_error);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TResult>.Default.GetHashCode(_result);
            hashCode = (hashCode * -1521134295) + _successful.GetHashCode();
            return hashCode;
        }
    }
}
