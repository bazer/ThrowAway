using Xunit;
using static ThrowAway.Option;

namespace ThrowAway.Tests
{
    public class OptionEitherTests
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

            static Option<int, string> GetThree() => 3;
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
                Assert.Equal(e.Failure, message);
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

            static Option<int, string> GetMessage() => null;
        }

        [Fact]
        public void CatchSome()
        {
            var some = Option.Catch(() => GetMessage());
            Assert.False(some.HasFailed);
            Assert.True(some.HasValue);
            Assert.Equal(3, some.Value);

            static Option<int, string> GetMessage() => 3;
        }

        [Fact]
        public void MatchSome()
        {
            var value = GetMessage()
                .Match(v => v.ToString(), f => f);

            Assert.Equal("3", value);

            static Option<int, string> GetMessage() => 3;
        }

        [Fact]
        public void MatchFail()
        {
            var value = GetMessage()
                .Match(v => v.ToString(), f => f);

            Assert.Equal("fail", value);

            static Option<int, string> GetMessage() => "fail";
        }

        [Fact]
        public void EqualsWithValues()
        {
            var some1 = Some<string, string>("some");
            var some2 = Some<string, string>("some");

            Assert.True(some1 == some2);
            Assert.True(some2 == some1);
            Assert.False(some1 != some2);
            Assert.False(some2 != some1);
        }

        [Fact]
        public void EqualsWithFails()
        {
            var fail1 = Fail<string, string>("fail");
            var fail2 = Fail<string, string>("fail");

            Assert.True(fail1 == fail2);
            Assert.True(fail2 == fail1);
            Assert.False(fail1 != fail2);
            Assert.False(fail2 != fail1);
        }

        [Fact]
        public void NotEqualsWithFailsDifferentType()
        {
            var fail1 = Fail<int, string>("fail");
            var fail2 = Fail<string, string>("fail");

            Assert.False(fail1.Equals(fail2));
            Assert.False(fail2.Equals(fail1));
        }

        [Fact]
        public void NotEqualsWithValues()
        {
            var some1 = Some<string, string>("some1");
            var some2 = Some<string, string>("some2");

            Assert.False(some1 == some2);
            Assert.False(some2 == some1);
            Assert.True(some1 != some2);
            Assert.True(some2 != some1);
        }

        [Fact]
        public void NotEqualsWithValueAndFail()
        {
            var some = Some<string, string>("some");
            var fail = Fail<string, string>("fail");

            Assert.False(some == fail);
            Assert.False(fail == some);
            Assert.True(some != fail);
            Assert.True(fail != some);
        }

        [Fact]
        public void NullDefault()
        {
            Option<string, string> defaultVal = default;

            Assert.False(defaultVal.HasValue);
            Assert.True(defaultVal.HasFailed);
            Assert.Null(defaultVal.Failure.Value);
            Assert.Null(defaultVal.Failure.StackTrace);
            Assert.Equal(string.Empty, defaultVal.Failure.ToString());
        }

        [Fact]
        public void StackTrace()
        {
            OptionConfig.LogStackTraceOnFailure = true;

            var fail = Fail<string, string>("fail");
            Assert.True(fail.HasFailed);
            Assert.Equal("fail", fail.Failure);
            Assert.NotNull(fail.Failure.StackTrace);
            Assert.NotEmpty(fail.Failure.StackTrace.ToString());

            OptionConfig.LogStackTraceOnFailure = false;
        }

        public interface ITest
        {
            int Value { get; }
        }

        public class Test : ITest
        {
            public int Value { get; set; }
        }

        [Fact]
        public void SomeReturnInterfaceValue()
        {
            var three = GetInterface();

            Assert.True(three.HasValue);
            Assert.Equal(3, three.Value.Value);

            static Option<ITest, string> GetInterface()
            {
                return new Test() { Value = 3 };
            };
        }

        [Fact]
        public void SomeReturnInterfaceType()
        {
            var three = GetInterface();

            Assert.True(three.HasValue);
            Assert.Equal(3, three.Value.Value);

            static Option<ITest, string> GetInterface()
            {
                return Option<ITest, string>.Some(new Test() { Value = 3 } as ITest);
            };
        }

    }
}