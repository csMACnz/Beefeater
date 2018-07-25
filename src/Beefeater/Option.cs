using System;
using System.Collections.Generic;

namespace Beefeater
{
    public struct Option<T> : IEquatable<Option<T>>
    {
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

        private readonly T _value;

        public Option(T value)
            : this()
        {
            if (value != null)
            {
                HasValue = true;
                _value = value;
            }
        }

        public static Option<T> None => default;

        public bool HasValue { get; }

        public static bool operator !=(Option<T> option1, Option<T> option2)
        {
            return !(option1 == option2);
        }

        public static bool operator ==(Option<T> option1, Option<T> option2)
        {
            return option1.Equals(option2);
        }

        public override bool Equals(object obj)
        {
            return obj is Option<T> && Equals((Option<T>)obj);
        }

        public bool Equals(Option<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            return -1939223833 + EqualityComparer<T>.Default.GetHashCode(_value);
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (some == null)
            {
                throw new ArgumentNullException(nameof(some));
            }

            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            return HasValue ? some(_value) : none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (some == null)
            {
                throw new ArgumentNullException(nameof(some));
            }

            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            if (HasValue)
            {
                some(_value);
            }
            else
            {
                none();
            }
        }

        public T ValueOr(T fallbackValue)
        {
            return !HasValue ? fallbackValue : _value;
        }

        public T ValueOrDefault()
        {
            return ValueOr(default);
        }
    }
}