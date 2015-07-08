using System;

namespace Beefeater
{
    public struct Option<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public Option(T value) : this()
        {
            if (value != null)
            {
                _hasValue = true;
                _value = value;
            }
        }

        public T ValueOr(T fallbackValue)
        {
            return !_hasValue ? fallbackValue : _value;
        }

        public static Option<T> None
        {
            get { return new Option<T>(); }
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");

            return _hasValue ? some(_value) : none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");
            if (_hasValue)
            {
                some(_value);
            }
            else
            {
                none();
            }
        }
    }
}