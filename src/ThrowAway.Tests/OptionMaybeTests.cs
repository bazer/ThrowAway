using System;
using ThrowAway.Extensions;
using Xunit;
using static ThrowAway.Option;

namespace ThrowAway.Tests
{
    public class OptionMaybeTests
    {
        [Fact]
        public void Void()
        {
            var voidValue = Option.Void();

            Assert.True(!voidValue.HasFailed);
            Assert.True(voidValue.HasValue);
            Assert.IsType<Unit>(voidValue.Value);
        }

        [Fact]
        public void SomeValueType()
        {
            var three = Option.Some(3);

            Assert.True(three.HasValue);
            Assert.Equal(3, three.Value);
        }

        [Fact]
        public void SomeStruct()
        {
            var now = DateTime.Now;
            var date = Option.Some(now);

            Assert.True(date.HasValue);
            Assert.Equal(now, date.Value);
        }

        [Fact]
        public void SomeReferenceType()
        {
            var @string = "123";
            var test = Option.Some(@string);

            Assert.True(test.HasValue);
            Assert.Equal(@string, test.Value);
        }

        [Fact]
        public void NoneValueType()
        {
            var test = Option<int>.Fail("fail");

            Assert.False(test.HasValue);
            Assert.Throws<HasFailedException<string>>(() => test.Value);
        }

        [Fact]
        public void NoneTest()
        {
            var test = Option.Fail("fail");

            Assert.False(test.HasValue);
            Assert.Throws<HasFailedException<string>>(() => test.Value);
        }

        [Fact]
        public void NoneStatic()
        {
            var test = Fail("fail");

            Assert.False(test.HasValue);
            Assert.Throws<HasFailedException<string>>(() => test.Value);
        }

        [Fact]
        public void SomeStatic()
        {
            var test = Some("test");

            Assert.True(test.HasValue);
            Assert.Equal("test", test.Value);
        }

        [Fact]
        public void SomeImplicit()
        {
            Assert.Equal("test", Some("test"));
        }

        [Fact]
        public void SomeExplicit()
        {
            Assert.Equal("test", (string)Some("test"));
        }

        [Fact]
        public void NoneImplicitFrom()
        {
            Assert.Throws<HasFailedException<string>>(() => { string test = Fail<string>("fail"); });
        }

        [Fact]
        public void NoneExplicitFrom()
        {
            Assert.Throws<HasFailedException<string>>(() => (string)Fail<string>("fail"));
        }

        [Fact]
        public void SomeNull()
        {
            Assert.Throws<ValueIsNullException>(() => Some<string>(null));
        }

        [Fact]
        public void NoneInsideSome()
        {
            var none = Fail("fail");
            var some = Some(none);
            Assert.True(some.HasValue);
            Assert.False(some.Value.HasValue);
        }

        [Fact]
        public void SomeInsideSome()
        {
            var innerSome = Some("123");
            var some = Some(innerSome);
            Assert.True(some.HasValue);
            Assert.True(some.Value.HasValue);
        }

        [Fact]
        public void NullableSome()
        {
            int? nullable = 3;
            var some = Some(nullable);
            Assert.True(some.HasValue);
            Assert.Equal(3, some.Value);
        }

        [Fact]
        public void NullableNone()
        {
            int? nullable = null;
            Assert.Throws<ValueIsNullException>(() => Some(nullable));
        }

        [Fact]
        public void SomeToString()
        {
            Assert.Equal("3", Some(3).ToString());
        }

        [Fact]
        public void NoneToString()
        {
            Assert.Equal("fail", Fail("fail").ToString());
        }

        [Fact]
        public void SomeStringImplicitTo()
        {
            Option<string> some = Some("value");
            Assert.True(some.HasValue);
            Assert.Equal("value", some);
        }

        [Fact]
        public void FailStringImplicitTo()
        {
            Option<string> some = Fail("fail");
            Assert.False(some.HasValue);
            Assert.Equal("fail", some.Failure);
        }

        [Fact]
        public void CatchFailWithThrow()
        {
            var message = "Fail message";

            var fail = Option.CatchFailure(() => GetMessage());
            Assert.True(fail.HasFailed);
            Assert.False(fail.HasValue);
            Assert.Equal(message, fail.Failure);

            Option<int> GetMessage()
            {
                var fail = Fail(message);
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

            Option<int> GetMessage() => GetMessage2().Value;
            Option<int> GetMessage2() => message;
        }

        [Fact]
        public void FailWithThrowNested()
        {
            var message = "Fail message";

            var fail = GetMessage();
            Assert.True(fail.HasFailed);
            Assert.Equal(message, fail.Failure);

            Option<int> GetMessage() => GetMessage2();
            Option<int> GetMessage2() => message;
        }

        [Fact]
        public void CatchFailWithThrowNull()
        {
            Assert.Throws<ValueIsNullException>(() =>
                Option.CatchFailure(() =>
                    GetMessage()));

            static Option<int> GetMessage() => null;
        }

        [Fact]
        public void CatchSome()
        {
            var some = Option.CatchFailure(() => GetMessage());
            Assert.False(some.HasFailed);
            Assert.True(some.HasValue);
            Assert.Equal(3, some.Value);

            static Option<int> GetMessage() => 3;
        }

        [Fact]
        public void MatchSome()
        {
            var value = GetMessage()
                .Match(v => v.ToString(), f => f);

            Assert.Equal("3", value);

            static Option<int> GetMessage() => 3;
        }

        [Fact]
        public void MatchFail()
        {
            var value = GetMessage()
                .Match(v => v.ToString(), f => f);

            Assert.Equal("fail", value);

            static Option<int> GetMessage() => "fail";
        }

        [Fact]
        public void EqualsWithValues()
        {
            var some1 = Some("some");
            var some2 = Some("some");

            Assert.True(some1 == some2);
            Assert.True(some2 == some1);
            Assert.False(some1 != some2);
            Assert.False(some2 != some1);
        }

        [Fact]
        public void EqualsWithFails()
        {
            var fail1 = Fail("fail");
            var fail2 = Fail("fail");

            Assert.True(fail1 == fail2);
            Assert.True(fail2 == fail1);
            Assert.False(fail1 != fail2);
            Assert.False(fail2 != fail1);
        }

        [Fact]
        public void NotEqualsWithFailsDifferentType()
        {
            var fail1 = Fail<int>("fail");
            var fail2 = Fail<string>("fail");

            Assert.False(fail1.Equals(fail2));
            Assert.False(fail2.Equals(fail1));
        }

        [Fact]
        public void NotEqualsWithValues()
        {
            var some1 = Some("some1");
            var some2 = Some("some2");

            Assert.False(some1 == some2);
            Assert.False(some2 == some1);
            Assert.True(some1 != some2);
            Assert.True(some2 != some1);
        }

        [Fact]
        public void NotEqualsWithValueAndFail()
        {
            var some = Some("some");
            var fail = Fail("fail");

            Assert.False(some == fail);
            Assert.False(fail == some);
            Assert.True(some != fail);
            Assert.True(fail != some);
        }

        [Fact]
        public void NullDefault()
        {
            Option<string> defaultVal = default;

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

            var fail = Fail("fail");
            Assert.True(fail.HasFailed);
            Assert.Equal("fail", fail.Failure);
            Assert.NotNull(fail.Failure.StackTrace);
            Assert.NotEmpty(fail.Failure.StackTrace.ToString());

            OptionConfig.LogStackTraceOnFailure = false;
        }

        [Fact]
        public void TryUnwrapMaybe_Success_OutParameters()
        {
            // Arrange: Create a successful Option (Maybe) instance.
            var opt = Some(42);

            // Act: Use the extension method to unwrap the value.
            bool result = opt.TryUnwrap(out int value, out string failure);

            // Assert: The unwrapping should succeed with the correct value and no failure message.
            Assert.True(result);
            Assert.Equal(42, value);
            Assert.Null(failure);
        }

        [Fact]
        public void TryUnwrapMaybe_Failure_OutParameters()
        {
            // Arrange: Create a failure Option.
            var opt = Option<int>.Fail("error occurred");

            // Act: Use the extension method to unwrap the Option.
            bool result = opt.TryUnwrap(out int value, out string failure);

            // Assert: The unwrapping should fail, returning the default value and the failure message.
            Assert.False(result);
            Assert.Equal(default(int), value);
            Assert.Equal("error occurred", failure);
        }

        [Fact]
        public void TryUnwrapMaybe_Success_Tuple()
        {
            // Arrange: Create a successful Option.
            var opt = Some("success");

            // Act: Use the tuple overload of the extension method.
            bool result = opt.TryUnwrap(out (string value, string failure) tupleResult);

            // Assert: The unwrapping should succeed with the expected tuple values.
            Assert.True(result);
            Assert.Equal("success", tupleResult.value);
            Assert.Null(tupleResult.failure);
        }

        [Fact]
        public void TryUnwrapMaybe_Failure_Tuple()
        {
            // Arrange: Create a failure Option.
            var opt = Fail<string>("error occurred");

            // Act: Use the tuple overload of the extension method.
            bool result = opt.TryUnwrap(out (string value, string failure) tupleResult);

            // Assert: The unwrapping should indicate failure, with the default value and the correct failure message.
            Assert.False(result);
            Assert.Equal(default(string), tupleResult.value);
            Assert.Equal("error occurred", tupleResult.failure);
        }

        [Fact]
        public void Use_OnSuccessfulOption_ExecutesAction()
        {
            var option = Option<int>.Some(42);
            int captured = 0;

            option.Use(x => captured = x);

            Assert.Equal(42, captured);
        }

        [Fact]
        public void Use_OnFailedOption_DoesNotExecuteAction()
        {
            var option = Option<int>.Fail("error");
            int captured = 0;

            option.Use(x => captured = x);

            Assert.Equal(0, captured); // The action should not run.
        }

        [Fact]
        public void SomeWhen_WithPredicateTrue_ReturnsSome()
        {
            int value = 10;
            var option = value.SomeWhen(x => x > 5, "value is too low");

            Assert.True(option.HasValue);
            Assert.Equal(10, option.Value);
        }

        [Fact]
        public void SomeWhen_WithPredicateFalse_ReturnsFail()
        {
            int value = 3;
            var option = value.SomeWhen(x => x > 5, "value is too low");

            Assert.True(option.HasFailed);
            Assert.Equal("value is too low", option.Failure);
        }

    }
}