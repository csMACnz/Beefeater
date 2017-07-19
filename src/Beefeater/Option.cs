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
    }
}