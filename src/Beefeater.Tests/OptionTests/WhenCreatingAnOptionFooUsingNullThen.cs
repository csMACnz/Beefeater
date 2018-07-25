using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField
namespace Beefeater.Tests.OptionTests
{
    public class WhenCreatingAnOptionFooUsingNullThen
    {
        private readonly Option<Foo> _option;

        public WhenCreatingAnOptionFooUsingNullThen()
        {
            _option = new Option<Foo>(null);
        }

        [Fact]
        public void HasValueReturnsFalse()
        {
            Assert.False(_option.HasValue);
        }

        [Fact]
        public void ValueOrNullMatchesNull()
        {
            Assert.Equal(null, _option.ValueOr(null));
        }

        [Fact]
        public void ValueOrNewFooMatchesNewFoo()
        {
            var newFoo = new Foo();
            Assert.Equal(newFoo, _option.ValueOr(newFoo));
        }

        [Fact]
        public void ExplicitCastToAnFooThrowsException()
        {
            Assert.Throws<PanicException>(() => (Foo)_option);
        }

        public class Foo
        {
        }
    }
}