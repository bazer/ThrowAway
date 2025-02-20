using System.Collections.Generic;
using ThrowAway.Extensions;
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

            var fail = Option.CatchFailure(() => GetMessage());
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

            var fail = Option.CatchFailure(() => GetMessage());
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
                Option.CatchFailure(() =>
                    GetMessage()));

            static Option<int, string> GetMessage() => null;
        }

        [Fact]
        public void CatchSome()
        {
            var some = Option.CatchFailure(() => GetMessage());
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
            }
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
            }
        }

        [Fact]
        public void SomeReturnInterfaceTypeHandle()
        {
            if (!GetInterface().TryUnwrap(out var three, out var f))
                Assert.Null(f);

            Assert.NotNull(three);
            Assert.Equal(3, three.Value);

            static Option<ITest> GetInterface()
            {
                return Option<ITest>.Some(new Test() { Value = 3 } as ITest);
            }
        }

        [Fact]
        public void SomeReturnInterfaceTypeHandlea()
        {
            if (GetInterface().TryUnwrap(out var result))
                Assert.Equal(3, result.value.Value);
            else
                Assert.Null(result.failure);

            Assert.NotNull(result.value);
            Assert.Equal(3, result.value.Value);

            static Option<ITest> GetInterface()
            {
                return Option<ITest>.Some(new Test() { Value = 3 } as ITest);
            }
        }

        [Fact]
        public void TryUnwrapEither_Success_OutParameters()
        {
            // Arrange: Create a successful Option using the Either pattern.
            Option<int, string> opt = Some<int, string>(100);

            // Act: Try to unwrap the Option using the out parameters overload.
            bool result = opt.TryUnwrap(out int value, out string failure);

            // Assert: The operation should succeed, with the correct value and no failure.
            Assert.True(result);
            Assert.Equal(100, value);
            Assert.Null(failure);
        }

        [Fact]
        public void TryUnwrapEither_Failure_OutParameters()
        {
            // Arrange: Create a failure Option.
            Option<int, string> opt = Fail<int, string>("error");

            // Act: Try to unwrap the Option.
            bool result = opt.TryUnwrap(out int value, out string failure);

            // Assert: The operation should fail, returning the default value and the correct failure message.
            Assert.False(result);
            Assert.Equal(default(int), value);
            Assert.Equal("error", failure);
        }

        [Fact]
        public void TryUnwrapEither_Success_Tuple()
        {
            // Arrange: Create a successful Option.
            Option<int, string> opt = Some<int, string>(55);

            // Act: Try to unwrap the Option using the tuple overload.
            bool result = opt.TryUnwrap(out (int value, string failure) res);

            // Assert: The operation should succeed and yield the expected tuple values.
            Assert.True(result);
            Assert.Equal(55, res.value);
            Assert.Null(res.failure);
        }

        [Fact]
        public void TryUnwrapEither_Failure_Tuple()
        {
            // Arrange: Create a failure Option.
            Option<int, string> opt = Fail<int, string>("failure case");

            // Act: Unwrap the Option into a tuple.
            bool result = opt.TryUnwrap(out (int value, string failure) res);

            // Assert: The unwrapping should indicate failure, returning default for the value and the failure message.
            Assert.False(result);
            Assert.Equal(default(int), res.value);
            Assert.Equal("failure case", res.failure);
        }

        [Fact]
        public void EitherTranspose_AllSuccessful_ReturnsListOfValues()
        {
            var options = new List<Option<int, string>> {
                Option<int, string>.Some(1),
                Option<int, string>.Some(2),
                Option<int, string>.Some(3)
            };

            var result = options.Transpose();

            Assert.True(result.HasValue);
            Assert.Equal(new List<int> { 1, 2, 3 }, result.Value);
        }

        [Fact]
        public void EitherTranspose_WithFailures_ReturnsListOfFailures()
        {
            var options = new List<Option<int, string>> {
                Option<int, string>.Some(1),
                Option<int, string>.Fail("error1"),
                Option<int, string>.Fail("error2")
            };

            var result = options.Transpose();

            Assert.True(result.HasFailed);
            Assert.Equal(new List<string> { "error1", "error2" }, result.Failure);
        }

        [Fact]
        public void MapFail_TransformsFailureCorrectly()
        {
            var option = Option<int, string>.Fail("error");
            var mapped = option.MapFail(f => f.Length);

            Assert.True(mapped.HasFailed);
            Assert.Equal(5, mapped.Failure); // "error".Length == 5
        }

        [Fact]
        public void ValueOrException_OnFailedOption_ThrowsCustomException()
        {
            var option = Option<int, string>.Fail("failure");
            var ex = Assert.Throws<HasFailedException<string>>(() => option.ValueOrException("Custom error message"));
            Assert.Contains("Custom error message", ex.Message);
        }

        [Fact]
        public void ValueOrException_OnSuccessfulOption_ReturnsValue()
        {
            var option = Option<int, string>.Some(100);
            int value = option.ValueOrException("Should not throw");
            Assert.Equal(100, value);
        }
    }
}