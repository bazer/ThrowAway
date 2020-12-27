using Xunit;
using static ThrowAway.Option;

namespace ThrowAway.Tests
{
    public class OptionFTests
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

            Option<int, string> GetThree() => 3;
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

            var fail = Option.Catch(() => GetMessage());
            Assert.True(fail.HasFailed);
            Assert.False(fail.HasValue);
            Assert.Equal(message, fail.Failure);

            Option<int, string> GetMessage()
            {
                var fail = Fail<int, string>(message);
                return fail.Value;
            }
        }

        [Fact]
        public void CatchFailWithThrowNested()
        {
            var message = "Fail message";

            var fail = Option.Catch(() => GetMessage());
            Assert.True(fail.HasFailed);
            Assert.Equal(message, fail.Failure);

            Option<int, string> GetMessage() => GetMessage2().Value;
            Option<int, string> GetMessage2() => message;
        }

        [Fact]
        public void FailWithThrowNested()
        {
            var message = "Fail message";

            var fail = GetMessage();
            Assert.True(fail.HasFailed);
            Assert.Equal(message, fail.Failure);

            Option<int, string> GetMessage() => GetMessage2();
            Option<int, string> GetMessage2() => message;
        }

        [Fact]
        public void CatchFailWithThrowNull()
        {
            Assert.Throws<ValueIsNullException>(() =>
                Option.Catch(() =>
                    GetMessage()));

            Option<int, string> GetMessage() => null;
        }

        [Fact]
        public void CatchSome()
        {
            var some = Option.Catch(() => GetMessage());
            Assert.False(some.HasFailed);
            Assert.True(some.HasValue);
            Assert.Equal(3, some.Value);

            Option<int, string> GetMessage() => 3;
        }

        [Fact]
        public void MatchSome()
        {
            var value = GetMessage()
                .Match(v => v.ToString(), f => f);

            Assert.Equal("3", value);

            Option<int, string> GetMessage() => 3;
        }

        [Fact]
        public void MatchFail()
        {
            var value = GetMessage()
                .Match(v => v.ToString(), f => f);

            Assert.Equal("fail", value);

            Option<int, string> GetMessage() => "fail";
        }
    }
}