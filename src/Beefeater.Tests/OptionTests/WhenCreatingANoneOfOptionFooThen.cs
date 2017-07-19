using Xunit;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Beefeater.Tests.OptionTests
{
    public class WhenCreatingANoneOfOptionFooThen
    {
        private readonly Option<Foo> _option;

        public WhenCreatingANoneOfOptionFooThen()
        {
            _option = Option<Foo>.None;
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
        public void ExplicitCastToAFooThrowsException()
        {
            Assert.Throws<PanicException>(() => (Foo)_option);
        }

        public class Foo
        {
        }
    }
}