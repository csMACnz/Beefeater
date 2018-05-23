using System;

namespace Beefeater
{
    public static class EitherExtensions
    {
        public static void Match<TResult1, TResult2>(
            this Either<TResult1, TResult2> result,
            Action<TResult1> processItem1,
            Action<TResult2> processItem2)
        {
            if (processItem1 == null) throw new ArgumentNullException(nameof(processItem1));
            if (processItem2 == null) throw new ArgumentNullException(nameof(processItem2));
            if (result.IsItem1)
            {
                processItem1(result.Item1);
            }
            else if (result.IsItem2)
            {
                processItem2(result.Item2);
            }
        }

        public static TValue Match<TResult1, TResult2, TValue>(
            this Either<TResult1, TResult2> result,
            Func<TResult1, TValue> processItem1,
            Func<TResult2, TValue> processItem2)
        {
            if (processItem1 == null) throw new ArgumentNullException(nameof(processItem1));
            if (processItem2 == null) throw new ArgumentNullException(nameof(processItem2));
            return result.IsItem1 ? processItem1(result.Item1) : processItem2(result.Item2);
        }
    }
}
