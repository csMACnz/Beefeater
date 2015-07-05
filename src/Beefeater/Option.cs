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
    }
}