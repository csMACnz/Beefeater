using System;

namespace Beefeater
{
    public static class ResultExtensions
    {
        public static void Match<TResult, TError>(
            this Result<TResult, TError> result,
            Action<TResult> some,
            Action<TError> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));
            if (result.Successful)
            {
                some(result.Value);
            }
            else
            {
                none(result.Error);
            }
        }

        public static TValue Match<TResult, TError, TValue>(
            this Result<TResult, TError> result,
            Func<TResult, TValue> some,
            Func<TError, TValue> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));
            return result.Successful ? some(result.Value) : none(result.Error);
        }
    }
}
