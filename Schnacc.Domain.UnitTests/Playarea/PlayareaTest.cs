namespace Schnacc.Domain.UnitTests.Playarea
{
    using FakeItEasy;

    using FluentAssertions;
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
        private void PlayareaSizeIsAtLeast4RowsAnd4Columns(int numberOfRows, int numberOfColumns)
        {
            // Arrange
            PlayareaSize size = new PlayareaSize(numberOfRows, numberOfColumns);
            FoodFactory factory = A.Dummy<FoodFactory>();

            // Act
            this.testee = new Playarea(size, factory);

            // Assert
            this.testee.Size.Should().BeEquivalentTo(new PlayareaSize(4, 4));
        }

        [Fact]
        private void NewlyCreatedPlaygroundShouldHaveStartGamestate()
        {
            // Act
            this.testee = new Playarea(new PlayareaSize(10, 10), A.Dummy<IFoodFactory>());

            // Assert
            this.testee.CurrentGameState.Should().Be(Game.Start);
        }

        [Theory]
        [InlineData(Direction.Right)]
        [InlineData(Direction.Left)]
        [InlineData(Direction.Up)]
        [InlineData(Direction.Down)]
        private void WhenDirectionOfSnakeSetPlaygroundShouldHaveRunningGamestate(Direction newFacingDirection)
        {
            // Arrange
            this.testee = new Playarea(new PlayareaSize(10, 10), A.Dummy<IFoodFactory>());

            // Act
            this.testee.UpdateSnakeDirection(newFacingDirection);

            // Assert
            this.testee.CurrentGameState.Should().Be(Game.Running);
        }

        [Fact]
        private void SnakeShouldNotMoveWhenDirectionIsNeverUpdated()
        {
            // Arrange
            PlayareaSize size = new PlayareaSize(10, 10);
            IFoodFactory foodFactory = A.Dummy<IFoodFactory>();
            this.testee = new Playarea(size, foodFactory);
            Position previousHeadPosition = this.testee.Snake.Head.Position;

            // Act
            this.testee.MoveSnakeWhenAllowed();

            // Assert
            this.testee.Snake.Head.Position.Should().BeEquivalentTo(previousHeadPosition);
        }
    }
}