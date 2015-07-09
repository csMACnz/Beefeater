using System;

namespace Beefeater
{
    public static class ResultExtensions
    {

        public static TValue Match<TResult, TError, TValue>(this Result<TResult, TError> result, Func<TResult, TValue> some, Func<TError, TValue> none)
        {
            if (some == null) throw new ArgumentNullException("some");
            if (none == null) throw new ArgumentNullException("none");
            return result.Successful ? some(result.Value) : none(result.Error);
        }
    }
}
