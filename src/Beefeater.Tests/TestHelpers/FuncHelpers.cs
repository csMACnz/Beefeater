using Xunit;

namespace Beefeater.Tests.TestHelpers
{
    public class FuncHelpers
    {
        public static bool ReturnTrue()
        {
            return true;
        }

        public static bool ReturnTrue<T>(T input)
        {
            return true;
        }

        public static bool ReturnFalse<T>(T input)
        {
            return false;
        }

        [Fact]
        public void ExerciseReturnTrue()
        {
            var result = ReturnTrue();
            Assert.True(result);
        }

        [Fact]
        public void ExerciseReturnTrueOfT()
        {
            var result = ReturnTrue("Hello World");
            Assert.True(result);
        }

        [Fact]
        public void ExerciseReturnFalseOfT()
        {
            var result = ReturnFalse("Hello World");
            Assert.False(result);
        }
    }
}