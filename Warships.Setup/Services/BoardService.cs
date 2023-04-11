using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Services
{
    internal class BoardService : IBoardService
    {
        internal BoardState BoardState;
        private readonly IBoardGenerator _boardGenerator;
        public BoardService(IBoardGenerator boardGenerator)
        {
            _boardGenerator = boardGenerator;
            BoardState = new BoardState
            {
                AvailablePoints = _boardGenerator.GenerateBoard()
            };
        }

        public Point GetRandomAvaiablePoint()
        {
            var rnd = new Random();
            var point = BoardState.AvailablePoints[rnd.Next(BoardState.AvailablePoints.Count)];
            return point;
        }

        public void RemoveOccupiedArea(Ship ship)
        {
            int startX = ship.Points.OrderBy(p => p.X).First().X - 1;
            int endX = ship.Points.OrderBy(p => p.X).Last().X + 1;
            int startY = ship.Points.OrderBy(p => p.Y).First().Y - 1;
            int endY = ship.Points.OrderBy(p => p.Y).Last().Y + 1;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    var point = BoardState.AvailablePoints.Where(p => p.X == x && p.Y == y).FirstOrDefault();
                    if (point != null)
                        BoardState.AvailablePoints.Remove(point);
                }
            }
        }


        public Point? DefineNextPoint(Point basePoint, BuildDirection direction)
        {
            Point? nextPoint = null;
            if (direction is BuildDirection.Horizontal)
                nextPoint = BoardState.AvailablePoints.Where(p => p.X == basePoint.X + 1 && p.Y == basePoint.Y).FirstOrDefault();
            else
                nextPoint = BoardState.AvailablePoints.Where(p => p.X == basePoint.X && p.Y == basePoint.Y + 1).FirstOrDefault();
            return nextPoint;
        }
    }
}
