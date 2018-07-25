using System;
using System.Collections.Generic;

namespace Beefeater
{
    public struct NotNull<T> : IEquatable<NotNull<T>>
    {
        public static implicit operator NotNull<T>(T item)
        {
            return new NotNull<T>(item);
        }

        public static implicit operator T(NotNull<T> item)
        {
            return item.Value;
        }

        private readonly bool _hasValue;
        private readonly T _value;

        public NotNull(T value)
            : this()
        {
            if (value == null)
            {
                throw new PanicException();
            }

            _value = value;
            _hasValue = true;
        }

        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new PanicException();
                }

                return _value;
            }
        }

        public static bool operator !=(NotNull<T> null1, NotNull<T> null2)
        {
            return !(null1 == null2);
        }

        public static bool operator ==(NotNull<T> null1, NotNull<T> null2)
        {
            return null1.Equals(null2);
        }

        public override bool Equals(object obj)
        {
            return obj is NotNull<T> && Equals((NotNull<T>)obj);
        }

        public bool Equals(NotNull<T> other)
        {
            return _hasValue == other._hasValue &&
                   EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            var hashCode = -1719897386;
            hashCode = (hashCode * -1521134295) + _hasValue.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(_value);
            return hashCode;
        }
    }
}