namespace Schnacc.Domain.UnitTests.Playarea
{
    using FakeItEasy;

    using FluentAssertions;

    using Schnacc.Domain;
    using Schnacc.Domain.Food;
    using Schnacc.Domain.Playarea;
    using Schnacc.Domain.Snake;

    using Xunit;

    public class PlayareaTest
    {
        private Playarea testee;

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 2)]
        [InlineData(4, 4)]
        [InlineData(4, 0)]
        private void playareaBoundariesAreAtLeast4RowsAnd4Columns(int row, int column)
        {
            // Arrange
            Position boundaries = new Position(row, column);
            FoodFactory factory = A.Dummy<FoodFactory>();
            Snake snake = A.Dummy<Snake>();

            // Act
            this.testee = new Playarea(boundaries, factory, snake);

            // Assert
            this.testee.Boundaries.Should().BeEquivalentTo(new Position(4,4));
        }

        [Fact]
        private void newlyCreatedPlaygroundShouldHaveStartGamestate()
        {
            // Act
            this.testee = new Playarea(new Position(10, 10), A.Dummy<IFoodFactory>(), new Snake(new Position(5, 5)));

            // Assert
            this.testee.CurrentGameState.Should().Be(Game.Start);
        }

        [Theory]
        [InlineData(Direction.Right)]
        [InlineData(Direction.Left)]
        [InlineData(Direction.Up)]
        [InlineData(Direction.Down)]
        private void whenDirectionOfSnakeSetPlaygroundShouldHaveRunningGamestate(Direction newFacingDirection)
        {
            // Arrange
            this.testee = new Playarea(new Position(10, 10), A.Dummy<IFoodFactory>(), new Snake(new Position(5, 5)));


            // Act
            this.testee.UpdateSnakeDirection(newFacingDirection);

            // Assert
            this.testee.CurrentGameState.Should().Be(Game.Running);
        }

        [Fact]
        private void snakeShouldNotMoveWhenDirectionIsNeverUpdated()
        {
            // Arrange
            Position boundaries = new Position(10, 10);
            IFoodFactory foodFactory = A.Dummy<IFoodFactory>();
            Snake snake = new Snake(new Position(5, 5));
            this.testee = new Playarea(boundaries, foodFactory, snake);
            Position previousHeadPosition = this.testee.Snake.Head.Position;

            // Act
            this.testee.MoveSnake();

            // Assert
            this.testee.Snake.Head.Position.Should().BeEquivalentTo(previousHeadPosition);
        }
    }
}