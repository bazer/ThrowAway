using System;
using Xunit;
using static ThrowAway.Option;

namespace ThrowAway.Tests
{
    public class OptionTests
    {
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
    }
}
