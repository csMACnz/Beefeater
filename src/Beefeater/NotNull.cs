namespace Beefeater
{
    public struct NotNull<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        public NotNull(T value) : this()
        {
            if (value == null) throw new PanicException();
            _value = value;
            _hasValue = true;
        }

        public T Value
        {
            get
            {
                if(!_hasValue)throw new PanicException();
                return _value;
            }
        }

        public static implicit operator NotNull<T>(T item)
        {
            return new NotNull<T>(item);
        }

        public static implicit operator T(NotNull<T> item)
        {
            return item.Value;
        }
    }
}