using System;

namespace Beefeater
{
    /// <summary>
    /// Represents a result that is either successful with a value or has an error.
    /// </summary>
    /// <typeparam name="TResult">The result, on success.</typeparam>
    /// <typeparam name="TError">The Error, on failure.</typeparam>
    public struct Result<TResult, TError>
    {
        private readonly TError _error;
        private readonly TResult _result;
        private readonly bool _successful;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TResult, TError}"/> class.
        /// </summary>
        /// <param name="result">The result on success.</param>
        private Result(TResult result)
        {
            _successful = true;
            _result = result;
            _error = default(TError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TResult, TError}"/> class.
        /// </summary>
        /// <param name="error">The error on failure.</param>
        private Result(TError error)
        {
            if(error == null) throw new ArgumentNullException("error");
            _successful = false;
            _error = error;
            _result = default(TResult);
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
                if (_successful) return _result;
                if (_error == null)
                {
                    throw new PanicException();
                }
                throw new InvalidOperationException("Result cannot be accessed on unsuccessful Result.");
            }
        }

        /// <summary>
        /// Gets the Error.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Error cannot be accessed on unsuccessful Result.</exception>
        public TError Error
        {
            get
            {
                if (_successful)
                    throw new InvalidOperationException("Error cannot be accessed on unsuccessful Result.");
                if (_error != null) return _error;
                throw new PanicException();
            }
        }

        public static Result<TResult, TError> OfValue(TResult value)
        {
            return new Result<TResult, TError>(value);
        }

        public static Result<TResult, TError> OfError(TError error)
        {
            return new Result<TResult, TError>(error);
        }
    }
}
