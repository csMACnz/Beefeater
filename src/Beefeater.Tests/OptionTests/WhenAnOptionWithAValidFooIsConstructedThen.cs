using Xunit;

// ReSharper disable ImpureMethodCallOnReadonlyValueField
namespace Beefeater.Tests.OptionTests
{
        public class WhenAnOptionWithAValidFooIsConstructedThen
        {
            private readonly Foo _foo;

            private readonly Option<Foo> _option;

            public WhenAnOptionWithAValidFooIsConstructedThen()
            {
                _foo = new Foo();

                _option = new Option<Foo>(_foo);
            }

            [Fact]
            public void HasValueReturnsTrue()
            {
                Assert.True(_option.HasValue);
            }

            [Fact]
            public void ValueOrNullMatchesOriginalFoo()
            {
                Assert.Equal(_foo, _option.ValueOr(null));
            }

            [Fact]
            public void ValueOrNewFooMatchesOriginalFoo()
            {
                Assert.Equal(_foo, _option.ValueOr(new Foo()));
            }

            [Fact]
            public void CanExplicitCastBackToAFoo()
            {
                var result = (Foo)_option;

                Assert.Equal(_foo, result);
            }

            public class Foo
            {
            }
        }
}
