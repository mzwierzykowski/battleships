using System.Collections;
using Warships.Setup.Config;

namespace Warships.Setup.Tests.Services
{
    public class BoardServiceTests
    {
        readonly BoardState _state;
        readonly BoardService _service;
        public BoardServiceTests()
        {
            _state = new BoardState();
            _service = new BoardService(_state);
            var boardDimension = new BoardDimension(10, 10);
            _service.GenerateBoard(boardDimension);
        }
        private class CentralShipGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 1),
                            new Point(1, 2),
                            new Point(1, 3),
                            new Point(1, 4),
                        }, "Destroyer")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 1),
                            new Point(2, 1),
                            new Point(3, 1),
                            new Point(4, 1),
                        }, "Destroyer")
                 };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 1),
                            new Point(2, 1),
                            new Point(3, 1),
                            new Point(4, 1),
                            new Point(5, 1),
                        }, "Battleship")
                };
                yield return new object[]
{
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 1),
                            new Point(1, 2),
                            new Point(1, 3),
                            new Point(1, 4),
                            new Point(1, 5),
                        }, "Battleship")
};
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(CentralShipGenerator))]
        public void RemoveOccupiedArea_CentralShipTest(Ship testModel)
        {
            // Example scenario
            // X - surrounding area
            // 0 - ship
            // ----------
            // X X X
            // X 0 X
            // X 0 X
            // X 0 X
            // X 0 X
            // X X X

            int expectedRemovedPointsCount = (testModel.Points.Count + 2) * 3;
            int expectedBoardSizeAfterRemoval = _state.AvailablePoints.Count - expectedRemovedPointsCount;

            Action action = () => _service.RemoveOccupiedArea(testModel);

            action.Should().NotThrow();
            _state.AvailablePoints.Should().HaveCount(expectedBoardSizeAfterRemoval);
        }

        private class EdgeShipGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 1),
                            new Point(0, 2),
                            new Point(0, 3),
                            new Point(0, 4),
                        }, "Destroyer")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 0),
                            new Point(2, 0),
                            new Point(3, 0),
                            new Point(4, 0),
                        }, "Destroyer")
                };
                yield return new object[]
{
                    new Ship(
                        new List<Point>
                        {
                            new Point(9, 1),
                            new Point(9, 2),
                            new Point(9, 3),
                            new Point(9, 4),
                        }, "Destroyer")
};
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 9),
                            new Point(2, 9),
                            new Point(3, 9),
                            new Point(4, 9),
                        }, "Destroyer")
                 };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 0),
                            new Point(2, 0),
                            new Point(3, 0),
                            new Point(4, 0),
                            new Point(5, 0),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 1),
                            new Point(0, 2),
                            new Point(0, 3),
                            new Point(0, 4),
                            new Point(0, 5),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(1, 9),
                            new Point(2, 9),
                            new Point(3, 9),
                            new Point(4, 9),
                            new Point(5, 9),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(9, 1),
                            new Point(9, 2),
                            new Point(9, 3),
                            new Point(9, 4),
                            new Point(9, 5),
                        }, "Battleship")
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(EdgeShipGenerator))]
        public void RemoveOccupiedArea_EdgeShipTest(Ship testModel)
        {
            // Example scenario
            // X - surrounding area
            // 0 - ship
            // ----------
            // X X
            // 0 X
            // 0 X
            // 0 X
            // 0 X
            // X X

            int expectedRemovedPointsCount = (testModel.Points.Count + 2) * 2;
            int expectedBoardSizeAfterRemoval = _state.AvailablePoints.Count - expectedRemovedPointsCount;

            Action action = () => _service.RemoveOccupiedArea(testModel);

            action.Should().NotThrow();
            _state.AvailablePoints.Should().HaveCount(expectedBoardSizeAfterRemoval);
        }

        private class CornerShipGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 0),
                            new Point(0, 1),
                            new Point(0, 2),
                            new Point(0, 3),
                        }, "Destroyer")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(2, 0),
                            new Point(3, 0),
                        }, "Destroyer")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(9, 0),
                            new Point(9, 1),
                            new Point(9, 2),
                            new Point(9, 3),
                        }, "Destroyer")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 9),
                            new Point(1, 9),
                            new Point(2, 9),
                            new Point(3, 9),
                        }, "Destroyer")
                 };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(9, 6),
                            new Point(9, 7),
                            new Point(9, 8),
                            new Point(9, 9),
                        }, "Destroyer")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(6, 9),
                            new Point(7, 9),
                            new Point(8, 9),
                            new Point(9, 9),
                        }, "Destroyer")
                 };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(2, 0),
                            new Point(3, 0),
                            new Point(4, 0),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 0),
                            new Point(0, 1),
                            new Point(0, 2),
                            new Point(0, 3),
                            new Point(0, 4),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(0, 9),
                            new Point(1, 9),
                            new Point(2, 9),
                            new Point(3, 9),
                            new Point(4, 9),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(9, 0),
                            new Point(9, 1),
                            new Point(9, 2),
                            new Point(9, 3),
                            new Point(9, 4),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(5, 9),
                            new Point(6, 9),
                            new Point(7, 9),
                            new Point(8, 9),
                            new Point(9, 9),
                        }, "Battleship")
                };
                yield return new object[]
                {
                    new Ship(
                        new List<Point>
                        {
                            new Point(9, 5),
                            new Point(9, 6),
                            new Point(9, 7),
                            new Point(9, 8),
                            new Point(9, 9),
                        }, "Battleship")
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(CornerShipGenerator))]
        public void RemoveOccupiedArea_CornerShipTest(Ship testModel)
        {
            // Example scenario
            // X - surrounding area
            // 0 - ship
            // ----------
            // 0 X
            // 0 X
            // 0 X
            // 0 X
            // X X

            int expectedRemovedPointsCount = (testModel.Points.Count + 1) * 2;
            int expectedBoardSizeAfterRemoval = _state.AvailablePoints.Count - expectedRemovedPointsCount;

            Action action = () => _service.RemoveOccupiedArea(testModel);

            action.Should().NotThrow();
            _state.AvailablePoints.Should().HaveCount(expectedBoardSizeAfterRemoval);
        }

        private class BoardDimensionGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new BoardDimension(1, 1)
                };
                yield return new object[]
                {
                    new BoardDimension(5, 5)
                };
                yield return new object[]
                {
                    new BoardDimension(10, 10)
                };
                yield return new object[]
                {
                    new BoardDimension(100, 100)
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        [Theory]
        [ClassData(typeof(BoardDimensionGenerator))]
        public void GenerateBoardTest(BoardDimension boardDimension)
        {
            int expectedBoardSize = boardDimension.Width * boardDimension.Height;

            Action action = () => _service.GenerateBoard(boardDimension);

            action.Should().NotThrow();
            _state.AvailablePoints.Should().HaveCount(expectedBoardSize);

        }

        [Fact]
        public void DefineNextPoint_Horizontal_ShouldDefineNext()
        {
            var testPoint = new Point(0, 0);
            Point? nextPoint = null;
            int expectedX = 1;
            int expectedY = 0;

            Action action = () => nextPoint = _service.DefineNextPoint(testPoint, BuildDirection.Horizontal);

            action.Should().NotThrow();
            nextPoint.Should().NotBeNull();
            nextPoint?.X.Should().Be(expectedX);
            nextPoint?.Y.Should().Be(expectedY);
        }

        [Fact]
        public void DefineNextPoint_Vertical_ShouldDefineNext()
        {
            var testPoint = new Point(0, 0);
            Point? nextPoint = null;
            int expectedX = 0;
            int expectedY = 1;

            Action action = () => nextPoint = _service.DefineNextPoint(testPoint, BuildDirection.Vertical);

            action.Should().NotThrow();
            nextPoint.Should().NotBeNull();
            nextPoint?.X.Should().Be(expectedX);
            nextPoint?.Y.Should().Be(expectedY);
        }

        [Theory]
        [InlineData(BuildDirection.Horizontal)]
        [InlineData(BuildDirection.Vertical)]
        public void DefineNextPoint_ShouldReturnNull(BuildDirection buildDirection)
        {
            var testPoint = new Point(9, 9);
            Point? nextPoint = null;

            Action action = () => nextPoint = _service.DefineNextPoint(testPoint, buildDirection);

            action.Should().NotThrow();
            nextPoint.Should().BeNull();
        }

        [Fact]
        public void GetRandomAvailablePoint_ShouldReturnPoint()
        {
            Point? point = null;

            Action action = () => point = _service.GetRandomAvaiablePoint();

            action.Should().NotThrow();
            point.Should().NotBeNull();

        }
    }
}
