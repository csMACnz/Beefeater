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

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public static Option<T> None
        {
            get { return new Option<T>(); }
        }
    }
}