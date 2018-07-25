using Xunit;

namespace Beefeater.Tests.TestHelpers
{
    public class ActionHelpers
    {
        public static void EmptyMethod()
        {
            // An intentionally empty test method
        }

        public static void EmptyMethod<T>(T v)
        {
            // An intentionally empty test method
        }

        [Fact]
        public void ExerciseEmptyMethod()
        {
            EmptyMethod();
        }

        [Fact]
        public void ExerciseEmptyMethodOfT()
        {
            EmptyMethod("Hello world");
        }
    }
}