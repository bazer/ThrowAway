using System;
using Xunit;

namespace ThrowAway.Extensions.Tests
{
    public class ValueOrExtensionsTests
    {
        [Fact]
        public void ValueOr_ShouldReturnValue_WhenOptionHasValue()
        {
            var option = Option<int>.Some(5);
            var result = option.ValueOr(10);
            Assert.Equal(5, result);
        }

        [Fact]
        public void ValueOr_ShouldReturnAlternative_WhenOptionIsNone()
        {
            var option = Option<int>.Fail("Error");
            var result = option.ValueOr(10);
            Assert.Equal(10, result);
        }
    }

    public class ValueOrWithFactoryTests
    {
        [Fact]
        public void ValueOrWithFactory_ShouldReturnValue_WhenOptionHasValue()
        {
            var option = Option<int, string>.Some(5);
            var result = option.ValueOr(() => 10);
            Assert.Equal(5, result);
        }

        [Fact]
        public void ValueOrWithFactory_ShouldReturnAlternative_WhenOptionIsNone()
        {
            var option = Option<int, string>.Fail("Error");
            var result = option.ValueOr(() => 10);
            Assert.Equal(10, result);
        }

        [Fact]
        public void ValueOrWithFactory_ShouldThrowException_WhenFactoryIsNull()
        {
            var option = Option<int, string>.Some(5);
            Assert.Throws<ArgumentNullException>(() => option.ValueOr(null as Func<int>));
        }
    }

    public class ValueOrWithFailureFactoryTests
    {
        [Fact]
        public void ValueOrWithFailureFactory_ShouldReturnValue_WhenOptionHasValue()
        {
            var option = Option<int, string>.Some(5);
            var result = option.ValueOr(failure => 10);
            Assert.Equal(5, result);
        }

        [Fact]
        public void ValueOrWithFailureFactory_ShouldReturnAlternative_WhenOptionIsNone()
        {
            var option = Option<int, string>.Fail("Error");
            var result = option.ValueOr(failure => failure.Length); // Example: using failure reason length as alternative value
            Assert.Equal(5, result); // "Error".Length == 5
        }

        [Fact]
        public void ValueOrWithFailureFactory_ShouldThrowException_WhenFactoryIsNull()
        {
            var option = Option<int, string>.Some(5);
            Assert.Throws<ArgumentNullException>(() => option.ValueOr(null as Func<string, int>));
        }
    }
}
