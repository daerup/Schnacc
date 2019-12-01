namespace Schnacc.Domain.UnitTests.Playarea
{
    using FakeItEasy;
    using FluentAssertions;
    using Schnacc.Domain.Food;
    using Schnacc.Domain.Snake;
    using Xunit;
    using Domain.Playarea;

    public class PlayareaTest
    {
        private Playarea testee;

        [Fact]
        private void newlyCreatedPlaygroundShouldHaveStartGamestate()
        {
            // Act
            this.testee = new Playarea(A.Dummy<FoodFactory>());

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
            this.testee = new Playarea(new FoodFactory((10, 10)));

            // Act
            this.testee.UpdateSnakeDirection(newFacingDirection);

            // Assert
            this.testee.CurrentGameState.Should().Be(Game.Running);
        }

        [Fact]
        private void snakeShouldNotMoveWhenDirectionIsNeverUpdated()
        {
            // Arrange
            this.testee = new Playarea(A.Dummy<FoodFactory>());
            Position previousHeadPosition = this.testee.Snake.Head.Position;

            // Act
            this.testee.MoveSnake();

            // Assert
            this.testee.Snake.Head.Position.Should().BeEquivalentTo(previousHeadPosition);
        }
    }
}