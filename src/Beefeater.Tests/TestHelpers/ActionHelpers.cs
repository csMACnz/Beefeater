using Xunit;

namespace Beefeater.Tests.TestHelpers
{
    public class ActionHelpers
    {
        public static void EmptyMethod()
        {
        }

        public static void EmptyMethod<T>(T v)
        {
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