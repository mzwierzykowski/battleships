using Microsoft.Extensions.Options;
using System.Configuration;
using Warships.API.Models;
using Warships.API.Validators;
using Warships.Configuration;
using Warships.Setup.Models;

namespace Warships.API.Tests.Validators
{
    public class RequestValidatorTests
    {
        [Fact]
        public void IsValid_ShouldThrow_MissingConfiguration()
        {
            bool isValid;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var boardDimensionOptions = Options.Create<BoardDimension>(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var validator = new RequestValidator(boardDimensionOptions);
            var request = new Request() { PointId = "dummy" };
            Action action = () => isValid = validator.IsValid(request);

            action.Should().ThrowExactly<ConfigurationErrorsException>().WithMessage(ExceptionMessages.MissingBoardDimensionConfiguration);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("a.b")]
        [InlineData("dummy")]
        [InlineData("1_1")]
        [InlineData("9.10")]
        [InlineData("10.9")]
        [InlineData("-1.0")]
        [InlineData("-1.-1")]
        public void IsValid_ShouldBeFalse(string pointId)
        {
            bool isValid = true;
            var boardDimension = new BoardDimension() { Height = 10, Width = 10 };
            var boardDimensionOptions = Options.Create<BoardDimension>(boardDimension);
            var validator = new RequestValidator(boardDimensionOptions);
            var request = new Request() { PointId = pointId };
            Action action = () => isValid = validator.IsValid(request);

            action.Should().NotThrow();
            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("0.0")]
        [InlineData("1.1")]
        [InlineData("3.5")]
        [InlineData("5.3")]
        [InlineData("9.9")]
        [InlineData("14.14")]
        public void IsValid_ShouldBTrue(string pointId)
        {
            bool isValid = true;
            var boardDimension = new BoardDimension() { Height = 15, Width = 15 };
            var boardDimensionOptions = Options.Create<BoardDimension>(boardDimension);
            var validator = new RequestValidator(boardDimensionOptions);
            var request = new Request() { PointId = pointId };
            Action action = () => isValid = validator.IsValid(request);

            action.Should().NotThrow();
            isValid.Should().BeTrue();
        }
    }
}
