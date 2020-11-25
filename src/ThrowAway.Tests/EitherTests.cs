using System;
using Xunit;
using static ThrowAway.Either;

namespace ThrowAway.Tests
{
    public class EitherTests
    {
        [Fact]
        public void SomeValueType()
        {
            var three = Some<int, string>(3);

            Assert.True(three.HasValue);
            Assert.Equal(3, three.Value);
        }

        [Fact]
        public void SomeFunction()
        {
            var three = GetThree();

            Assert.True(three.HasValue);
            Assert.Equal(3, three.Value);

            Either<int, string> GetThree() => 3;
        }

        [Fact]
        public void SomeImplicit()
        {
            Assert.Equal("test", Some<string, int>("test"));
        }

        [Fact]
        public void SomeExplicit()
        {
            Assert.Equal("test", (string)Some<string, int>("test"));
        }
    }
}
