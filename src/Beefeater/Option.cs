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

        public bool HasValue { get { return _hasValue; } }

        public static explicit operator T(Option<T> option)
        {
            if (option._hasValue)
            {
                return option._value;
            }

            var t = typeof(T);
            if (Nullable.GetUnderlyingType(t) != null)
            {
                // T is a Nullable<>
                return default(T);
            }

            throw new PanicException();
        }
    }
}