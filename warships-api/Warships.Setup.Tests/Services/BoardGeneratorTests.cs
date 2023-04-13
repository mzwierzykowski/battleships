using Microsoft.Extensions.Options;
using System.Collections;
using System.Configuration;
using Warships.Configuration;

namespace Warships.Setup.Tests.Services
{
    public class BoardGeneratorTests
    {
        [Fact]
        public void GenearteBoard_MissingConfiguration_ShouldThrow()
        {
            List<Point> board = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var boardDimensionOptions = Options.Create<BoardDimension>(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var service = new BoardGenerator(boardDimensionOptions);
            Action action = () => board = service.GenerateBoard();

            action.Should().ThrowExactly<ConfigurationErrorsException>().WithMessage(ExceptionMessages.MissingBoardDimensionConfiguration);
        }
        private class BoardDimensionGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new BoardDimension() { Width = 1, Height = 1 },
                };
                yield return new object[]
                {
                    new BoardDimension() { Width = 5, Height = 5 },
                };
                yield return new object[]
                {
                    new BoardDimension() { Width = 10, Height = 10 },
                };
                yield return new object[]
                {
                    new BoardDimension() { Width = 100, Height = 100 }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        [Theory]
        [ClassData(typeof(BoardDimensionGenerator))]
        public void GenerateBoard_ShouldCreateBoard(BoardDimension boardDimension)
        {
            int expectedBoardSize = boardDimension.Width * boardDimension.Height;
            var boardDimensionOptions = Options.Create(boardDimension);
            var service = new BoardGenerator(boardDimensionOptions);
            List<Point>? board = null;

            Action action = () => board = service.GenerateBoard();

            action.Should().NotThrow();
            board.Should().NotBeNull();
            board?.Count.Should().Be(expectedBoardSize);

        }
    }
}
