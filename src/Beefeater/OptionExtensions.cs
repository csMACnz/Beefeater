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

        public static Option<T> Unbox<T>(this Option<T?> nullableOption) where T : struct
        {
            var nullable = nullableOption.ValueOr(null);
            if (nullable.HasValue)
            {
                return new Option<T>(nullable.Value);
            }
            return Option<T>.None;
        }
    }
}