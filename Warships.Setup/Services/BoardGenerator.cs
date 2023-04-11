using Microsoft.Extensions.Options;
using System.Configuration;
using Warships.Setup.Config;
using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Services
{
    internal class BoardGenerator : IBoardGenerator
    {
        private readonly BoardDimension _boardDimension;
        public BoardGenerator(IOptions<BoardDimension> boardDimension) 
        {
            _boardDimension = boardDimension.Value;
        }

        public List<Point> GenerateBoard()
        {
            if (_boardDimension == null)
                throw new ConfigurationErrorsException(ExceptionMessages.MissingBoardDimensionConfiguration);

            var board = new List<Point>();
            for (int y = 0; y < _boardDimension.Height; y++)
            {
                for (int x = 0; x < _boardDimension.Width; x++)
                {
                    board.Add(new Point(x, y));
                }
            }
            return board;
        }
    }
}
