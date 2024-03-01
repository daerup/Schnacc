using FakeItEasy;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Playarea
{
    public class PlayareaTest
    {
        private Domain.Playarea.Playarea testee;

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 2)]
        [InlineData(4, 4)]
        [InlineData(4, 0)]
        private void PlayareaSizeIsAtLeast4RowsAnd4Columns(int numberOfRows, int numberOfColumns)
        {
            // Arrange
            var size = new PlayareaSize(numberOfRows, numberOfColumns);
            var factory = A.Dummy<FoodFactory>();

            // Act
            this.testee = new Domain.Playarea.Playarea(size, factory);

            // Assert
            this.testee.Size.Should().BeEquivalentTo(new PlayareaSize(4, 4));
        }

        [Fact]
        private void NewlyCreatedPlaygroundShouldHaveStartGamestate()
        {
            // Act
            this.testee = new Domain.Playarea.Playarea(new PlayareaSize(10, 10), A.Dummy<IFoodFactory>());

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
            this.testee = new Domain.Playarea.Playarea(new PlayareaSize(10, 10), A.Dummy<IFoodFactory>());

            // Act
            this.testee.UpdateSnakeDirection(newFacingDirection);

            // Assert
            this.testee.CurrentGameState.Should().Be(Game.Running);
        }

        [Fact]
        private void SnakeShouldNotMoveWhenDirectionIsNeverUpdated()
        {
            // Arrange
            var size = new PlayareaSize(10, 10);
            var foodFactory = A.Dummy<IFoodFactory>();
            this.testee = new Domain.Playarea.Playarea(size, foodFactory);
            Position previousHeadPosition = this.testee.Snake.Head.Position;

            // Act
            this.testee.MoveSnakeWhenAllowed();

            // Assert
            this.testee.Snake.Head.Position.Should().BeEquivalentTo(previousHeadPosition);
        }
    }
}