using System;

namespace Beefeater
{
    /// <summary>
    /// Represents a result that is either <see cref="TResult1"/> or <see cref="TResult2"/>.
    /// This is simulating a Union from other languages.
    /// </summary>
    /// <typeparam name="TResult">The result, if is <see cref="TResult1"/>.</typeparam>
    /// <typeparam name="TResult2">The result, if is <see cref="TResult2"/>.</typeparam>
    public struct Either<TResult1, TResult2>
    {
        private readonly TResult1 _result1;
        private readonly TResult2 _result2;
        private readonly State _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TResult1, TResult2}"/> class.
        /// </summary>
        /// <param name="value">The <see cref="TResult1"/> value.</param>
        private Either(TResult1 value)
        {
            _state = State.Result1;
            _result1 = value;
            _result2 = default(TResult2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TResult1, TResult2}"/> class.
        /// </summary>
        /// <param name="value">The <see cref="TResult2"/> value.</param>
        private Either(TResult2 value)
        {
            _state = State.Result2;
            _result1 = default(TResult1);
            _result2 = value;
        }

        public bool HasValue
        {
            get
            {
                return
                    (_state == State.Result1 && _result1 != null) ||
                    (_state == State.Result2 && _result2 != null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Either{TResult1, TResult2}"/> is of type <see cref="TResult1"/>.
        /// </summary>
        public bool IsItem1
        {
            get
            {
                return _state == State.Result1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Either{TResult1, TResult2}"/> is of type <see cref="TResult2"/>.
        /// </summary>
        public bool IsItem2
        {
            get
            {
                return _state == State.Result2;
            }
        }

        /// <summary>
        /// Gets the result of type <see cref="TResult1"/>.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Item1 cannot be accessed on Either of Item2.</exception>
        public TResult1 Item1
        {
            get
            {
                if (_state != State.Result1)
                    throw new InvalidOperationException("Item1 cannot be accessed on Either of Item2.");
                return _result1;
            }
        }

        /// <summary>
        /// Gets the result of type <see cref="TResult1"/>.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Item2 cannot be accessed on Either of Item1.</exception>
        public TResult2 Item2
        {
            get
            {
                if (_state != State.Result2)
                    throw new InvalidOperationException("Item2 cannot be accessed on Either of Item1.");
                return _result2;
            }
        }

        public static implicit operator Either<TResult1, TResult2>(TResult1 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        public static implicit operator Either<TResult1, TResult2>(TResult2 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        public static Either<TResult1, TResult2> OfResult1(TResult1 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        public static Either<TResult1, TResult2> OfResult2(TResult2 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        private enum State
        {
            Result1,
            Result2
        }
    }
}
