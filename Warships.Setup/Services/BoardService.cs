using Warships.Setup.Config;
using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Services
{
    internal class BoardService : IBoardService
    {
        private readonly BoardState _boardState;
        public BoardService(BoardState boardState)
        {
            _boardState = boardState;
        }

        public void GenerateBoard(BoardDimension boardDimension)
        {
            _boardState.AvailablePoints = new List<Point>();
            for (int y = 0; y < boardDimension.Height; y++)
            {
                for (int x = 0; x < boardDimension.Width; x++)
                {
                    _boardState.AvailablePoints.Add(new Point(x, y));
                }
            }
        }

        public Point GetRandomAvaiablePoint()
        {
            var rnd = new Random();
            var point = _boardState.AvailablePoints[rnd.Next(_boardState.AvailablePoints.Count)];
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
                    var point = _boardState.AvailablePoints.Where(p => p.X == x && p.Y == y).FirstOrDefault();
                    if (point != null)
                        _boardState.AvailablePoints.Remove(point);
                }
            }
        }


        public Point? DefineNextPoint(Point basePoint, BuildDirection direction)
        {
            Point? nextPoint = null;
            if (direction is BuildDirection.Horizontal)
                nextPoint = _boardState.AvailablePoints.Where(p => p.X == basePoint.X + 1 && p.Y == basePoint.Y).FirstOrDefault();
            else
                nextPoint = _boardState.AvailablePoints.Where(p => p.X == basePoint.X && p.Y == basePoint.Y + 1).FirstOrDefault();
            return nextPoint;
        }
    }
}
