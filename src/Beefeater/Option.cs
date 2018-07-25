using System;

namespace Beefeater
{
    public struct Option<T>
    {
        private readonly T _value;

        public Option(T value) : this()
        {
            if (value != null)
            {
                HasValue = true;
                _value = value;
            }
        }

        public T ValueOr(T fallbackValue)
        {
            return !HasValue ? fallbackValue : _value;
        }

        public static Option<T> None => new Option<T>();

        public bool HasValue { get; }

        public static implicit operator Option<T>(T item)
        {
            return new Option<T>(item);
        }

        public static explicit operator T(Option<T> option)
        {
            if (option.HasValue)
            {
                return option._value;
            }

            throw new PanicException();
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return HasValue ? some(_value) : none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));
            if (HasValue)
            {
                some(_value);
            }
            else
            {
                none();
            }
        }

        public T ValueOrDefault()
        {
            return ValueOr(default(T));
        }
    }
}