using Moq;
using System.Collections;
using Warships.Configuration;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Tests.Services
{
    public class BoardServiceTests
    {
        readonly BoardService _service;
        readonly BoardState _state;
        private int _boardGeneratorCallCount = 0;
        private readonly BoardDimension _boardDimension = new() { Height = 10, Width = 10 };
        public BoardServiceTests()
        {
            var boardGeneratorMock = new Mock<IBoardGenerator>();
            boardGeneratorMock.Setup(x => x.GenerateBoard())
                .Returns(() =>
                {
                    
                    var board = new List<Point>();
                    for (int y = 0; y < _boardDimension.Height; y++)
                    {
                        for (int x = 0; x < _boardDimension.Width; x++)
                        {
                            board.Add(new Point(x, y));
                        }
                    }
                    return board;
                })
                .Callback(() =>
                {
                    _boardGeneratorCallCount++;
                });
            _service = new BoardService(boardGeneratorMock.Object);
            _state = _service.BoardState;
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
        internal void DefineNextPoint_Vertical_ShouldDefineNext()
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
        internal void DefineNextPoint_ShouldReturnNull(BuildDirection buildDirection)
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

        [Fact]
        public void ResetBoardState_ShouldPrepareNewBoard()
        {
            _boardGeneratorCallCount = 0;
            int expectedCallbackCount = 1;
            int expectedAvailablePointsCount = _boardDimension.Width * _boardDimension.Height;
            _service.BoardState = new BoardState()
            {
                AvailablePoints = new List<Point>()
                {
                    new Point(1,1)
                }
            };
            Action action = () => _service.ResetBoardState();
            action.Should().NotThrow();
            _service.BoardState.Should().NotBeNull();
            _service.BoardState?.AvailablePoints.Should().NotBeNullOrEmpty();
            _service.BoardState?.AvailablePoints.Should().HaveCount(expectedAvailablePointsCount);
            _boardGeneratorCallCount.Should().Be(expectedCallbackCount);


        }
    }
}
