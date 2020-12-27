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

        [Fact]
        public void CatchFail()
        {
            var message = "Fail message";
            try
            {
                var fail = Fail<int, string>(message);
                Assert.True(fail.HasFailed);
                Assert.False(fail.HasValue);
                Assert.False(fail.HasNone);

                var val = fail.Value;
            }
            catch (HasFailedException<string> e)
            {
                Assert.Equal(e.Value, message);
            }
        }

        [Fact]
        public void CatchFailWithThrow()
        {
            var message = "Fail message";

            var fail = Either.Catch(() => GetMessage());
            Assert.True(fail.HasFailed);
            Assert.False(fail.HasValue);
            Assert.False(fail.HasNone);
            Assert.Equal(message, fail.Failure);

            Either<int, string> GetMessage()
            {
                var fail = Fail<int, string>(message);
                return fail.Value;
            }
        }

        [Fact]
        public void CatchFailWithThrowImplicit()
        {
            var message = "Fail message";

            var fail = Either.Catch(() => GetMessage());
            Assert.True(fail.HasFailed);
            Assert.Equal(message, fail.Failure);

            Either<int, string> GetMessage() => message;
        }

        [Fact]
        public void CatchFailWithThrowNull()
        {
            var none = Either.Catch(() => GetMessage());
            Assert.False(none.HasFailed);
            Assert.False(none.HasValue);
            Assert.True(none.HasNone);

            Either<int, string> GetMessage() => null;
        }

        [Fact]
        public void CatchSome()
        {
            var some = Either.Catch(() => GetMessage());
            Assert.False(some.HasFailed);
            Assert.True(some.HasValue);
            Assert.False(some.HasNone);
            Assert.Equal(3, some.Value);

            Either<int, string> GetMessage() => 3;
        }

        //[Fact]
        //public void CatchSomeMatch()
        //{
        //    var value = Either.Catch(() => GetMessage()).Match(
        //        value => value)

        //    Assert.False(value.HasFailed);
        //    Assert.True(value.HasValue);
        //    Assert.False(value.HasNone);
        //    Assert.Equal(3, value.Value);

        //    Either<int, string> GetMessage() => 3;
        //}
    }
}
