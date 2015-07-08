using System;

namespace Beefeater
{
    /// <summary>
    /// Represents a result that is either successful or has an exception.
    /// </summary>
    /// <typeparam name="TResult">The result, on success.</typeparam>
    /// <typeparam name="TException">The Exception, on failure.</typeparam>
    public struct Result<TResult, TException>
    {
        private readonly TException _exception;
        private readonly TResult _result;
        private readonly bool _successful;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TResult, TException}"/> class.
        /// </summary>
        /// <param name="result">The result on success.</param>
        public Result(TResult result)
        {
            _successful = true;
            _result = result;
            _exception = default(TException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TResult, TException}"/> class.
        /// </summary>
        /// <param name="error">The error on failure.</param>
        public Result(TException error)
        {
            if(error == null) throw new ArgumentNullException("error");
            _successful = false;
            _exception = error;
            _result = default(TResult);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Result{TResult, TException}"/> is successful.
        /// </summary>
        public bool Successful
        {
            get
            {
                if (!_successful && _exception == null)
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
                if (_exception == null)
                {
                    throw new PanicException();
                }
                throw new InvalidOperationException("Result cannot be accessed on unsuccessful Result.");
            }
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Exception cannot be accessed on unsuccessful Result.</exception>
        public TException Exception
        {
            get
            {
                if (_successful)
                    throw new InvalidOperationException("Exception cannot be accessed on unsuccessful Result.");
                if (_exception != null) return _exception;
                throw new PanicException();
            }
        }

        public TValue Match<TValue>(Func<TResult, TValue> some, Func<TException, TValue> none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");
            return Successful ? some(Value) : none(Exception);
        }
    }
}
