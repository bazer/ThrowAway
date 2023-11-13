using System;
using ThrowAway.Extensions;
using Xunit;

namespace ThrowAway.Tests
{
    public class ElseTests
    {
        [Fact]
        public void Else_ReturnsCurrentOption_WhenSuccessful()
        {
            // Arrange
            var option = Option<int, string>.Some(42);
            var alternativeOption = Option<int, string>.Some(24);

            // Act
            var result = option.Else(alternativeOption);

            // Assert
            Assert.Equal(option, result);
        }

        [Fact]
        public void Else_ReturnsAlternativeOption_WhenFailed()
        {
            // Arrange
            var option = Option<int, string>.Fail("fail");
            var alternativeOption = Option<int, string>.Some(24);

            // Act
            var result = option.Else(alternativeOption);

            // Assert
            Assert.Equal(alternativeOption, result);
        }

        [Fact]
        public void Else_ThrowsArgumentNullException_WhenAlternativeOptionFactoryIsNull()
        {
            // Arrange
            var option = Option<int, string>.Fail("fail");

            Func<Option<int, string>> nullValue = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => option.Else(nullValue));
        }

        [Fact]
        public void Else_ReturnsCurrentOption_WhenSuccessful_WithFactory()
        {
            // Arrange
            var option = Option<int, string>.Some(42);

            // Act
            var result = option.Else(() => Option<int, string>.Some(24));

            // Assert
            Assert.Equal(option, result);
        }

        [Fact]
        public void Else_ReturnsAlternativeOption_WhenFailed_WithFactory()
        {
            // Arrange
            var option = Option<int, string>.Fail("fail");

            // Act
            var result = option.Else(() => Option<int, string>.Some(24));

            // Assert
            Assert.Equal(Option<int, string>.Some(24), result);
        }
    }
}
