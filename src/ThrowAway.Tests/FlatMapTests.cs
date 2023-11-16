using System;
using ThrowAway.Extensions;
using Xunit;

namespace ThrowAway.Tests
{
    public class FlatMapTests
    {

        [Fact]
        public void FlatMap_ReturnsMappedOption_WhenOptionIsSome()
        {
            // Arrange
            var option = Option.Some<int, string>(5);

            // Act
            var result = option.FlatMap(x => Option.Some<string, string>(x.ToString()));

            // Assert
            Assert.Equal("5", result.Value);
        }

        [Fact]
        public void FlatMap_ReturnsFail_WhenOptionIsFail()
        {
            // Arrange
            var option = Option.Fail<int, string>("error");

            // Act
            var result = option.FlatMap(x => Option.Some<string, string>(x.ToString()));

            // Assert
            Assert.Equal("error", result.Failure);
        }

        [Fact]
        public void FlatMap_ThrowsArgumentNullException_WhenMappingIsNull()
        {
            // Arrange
            var option = Option.Some<int, string>(5);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => option.FlatMap<string>(null));
        }
    }
}
