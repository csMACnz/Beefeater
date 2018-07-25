using System;
using System.Collections.Generic;

namespace Beefeater
{
    /// <summary>
    /// Represents a result that is either <see cref="TResult1"/> or <see cref="TResult2"/>.
    /// This is simulating a Union from other languages.
    /// </summary>
    /// <typeparam name="TResult1">The result, if is <see cref="TResult1"/>.</typeparam>
    /// <typeparam name="TResult2">The result, if is <see cref="TResult2"/>.</typeparam>
    public struct Either<TResult1, TResult2> : IEquatable<Either<TResult1, TResult2>>
    {
        public static implicit operator Either<TResult1, TResult2>(TResult1 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        public static implicit operator Either<TResult1, TResult2>(TResult2 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        private readonly TResult1 _result1;
        private readonly TResult2 _result2;
        private readonly State _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TResult1, TResult2}"/> struct.
        /// </summary>
        /// <param name="value">The <see cref="TResult1"/> value.</param>
        private Either(TResult1 value)
        {
            _state = State.Result1;
            _result1 = value;
            _result2 = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TResult1, TResult2}"/> struct.
        /// </summary>
        /// <param name="value">The <see cref="TResult2"/> value.</param>
        private Either(TResult2 value)
        {
            _state = State.Result2;
            _result1 = default;
            _result2 = value;
        }

        private enum State
        {
            Result1,
            Result2
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
                {
                    throw new InvalidOperationException("Item1 cannot be accessed on Either of Item2.");
                }

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
                {
                    throw new InvalidOperationException("Item2 cannot be accessed on Either of Item1.");
                }

                return _result2;
            }
        }

        public static bool operator !=(Either<TResult1, TResult2> left, Either<TResult1, TResult2> right)
        {
            return !(left == right);
        }

        public static bool operator ==(Either<TResult1, TResult2> left, Either<TResult1, TResult2> right)
        {
            return left.Equals(right);
        }

        public static Either<TResult1, TResult2> OfResult1(TResult1 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        public static Either<TResult1, TResult2> OfResult2(TResult2 value)
        {
            return new Either<TResult1, TResult2>(value);
        }

        public bool Equals(Either<TResult1, TResult2> other)
        {
            return Equals(_state, other._state) && Equals(_result1, other._result1) && Equals(_result2, other._result2);
        }

        public override bool Equals(object obj)
        {
            return obj is Either<TResult1, TResult2> && Equals((Either<TResult1, TResult2>)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 1830942300;
            hashCode = (hashCode * -1521134295) + EqualityComparer<TResult1>.Default.GetHashCode(_result1);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TResult2>.Default.GetHashCode(_result2);
            hashCode = (hashCode * -1521134295) + _state.GetHashCode();
            return hashCode;
        }

        public void Match(
            Action<TResult1> processItem1,
            Action<TResult2> processItem2)
        {
            if (processItem1 == null)
            {
                throw new ArgumentNullException(nameof(processItem1));
            }

            if (processItem2 == null)
            {
                throw new ArgumentNullException(nameof(processItem2));
            }

            if (IsItem1)
            {
                processItem1(Item1);
            }
            else if (IsItem2)
            {
                processItem2(Item2);
            }
        }

        public TValue Match<TValue>(
            Func<TResult1, TValue> processItem1,
            Func<TResult2, TValue> processItem2)
        {
            if (processItem1 == null)
            {
                throw new ArgumentNullException(nameof(processItem1));
            }

            if (processItem2 == null)
            {
                throw new ArgumentNullException(nameof(processItem2));
            }

            return IsItem1 ? processItem1(Item1) : processItem2(Item2);
        }
    }
}
