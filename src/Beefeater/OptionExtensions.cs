using System;

namespace Beefeater
{
    public static class OptionExtensions
    {
        public static Option<T> AsAnOption<T>(this T? nullable) where T : struct
        {
            if (nullable.HasValue)
            {
                return new Option<T>(nullable.Value);
            }
            return Option<T>.None;
        }

        public static TResult Match<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> some, Func<TResult> none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");

            return option.HasValue ? some((TValue)option) : none();
        }

        public static void Match<TValue>(this Option<TValue> option, Action<TValue> some, Action none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");
            if (option.HasValue)
            {
                some((TValue)option);
            }
            else
            {
                none();
            }
        }

        public static T? ToNullable<T>(this Option<T> option) where T : struct
        {
            T? result = null;
            if (option.HasValue)
            {
                result = (T)option;
            }
            return result;
        }

        public static Option<T> Unbox<T>(this Option<T?> nullableOption) where T : struct
        {
            var nullable = nullableOption.ValueOr(null);
            if (nullable.HasValue)
            {
                return new Option<T>(nullable.Value);
            }
            return Option<T>.None;
        }

        public static T ValueOrDefault<T>(this Option<T> option)
        {
            return option.ValueOr(default(T));
        }
    }
}