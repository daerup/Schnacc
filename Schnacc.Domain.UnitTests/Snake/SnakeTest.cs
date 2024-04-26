using System.Linq;
using FluentAssertions;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Snake
{
    public class SnakeTest
    {
        private readonly Domain.Snake.Snake testee = new Domain.Snake.Snake(new Position(0, 0));

        [Fact]
        public void ForwardsFacingSnakeShouldMoveFarwards()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.Right);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(0);
            this.testee.Head.Position.Column.Should().Be(1);
        }

        [Fact]
        public void BackwardsFacingSnakeShouldMoveBackwards()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.Left);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(0);
            this.testee.Head.Position.Column.Should().Be(-1);
        }

        [Fact]
        public void UpwardsFacingSnakeShouldMoveUpwards()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.Up);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(-1);
            this.testee.Head.Position.Column.Should().Be(0);
        }

        [Fact]
        public void DownwardsFacingSnakeShouldMoveDownwards()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.Down);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(1);
            this.testee.Head.Position.Column.Should().Be(0);
        }

        [Fact]
        public void StillStandingSnakeShouldNotMove()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.None);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(0);
            this.testee.Head.Position.Column.Should().Be(0);
        }

        [Theory]
        [InlineData(Direction.Right, Direction.Up)]
        [InlineData(Direction.Right, Direction.Down)]
        [InlineData(Direction.Left, Direction.Up)]
        [InlineData(Direction.Left, Direction.Down)]
        [InlineData(Direction.Up, Direction.Right)]
        [InlineData(Direction.Up, Direction.Left)]
        [InlineData(Direction.Down, Direction.Right)]
        [InlineData(Direction.Down, Direction.Left)]
        public void SnakeShouldBeAbleToChangeDirectionBy90Degrees(Direction startDirection, Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(startDirection);

            // Act
            this.testee.UpdateFacingDirection(newDirection);

            // Assert
            this.testee.CurrentDirection.Should().Be(newDirection);
        }

        [Theory]
        [InlineData(Direction.Right, Direction.Left)]
        [InlineData(Direction.Left, Direction.Right)]
        [InlineData(Direction.Up, Direction.Down)]
        [InlineData(Direction.Down, Direction.Up)]
        public void SnakeShouldNotBeAbleToChangeDirectionBy180Degrees(Direction startDirection, Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(startDirection);

            // Act
            this.testee.UpdateFacingDirection(newDirection);

            // Assert
            this.testee.CurrentDirection.Should().NotBe(newDirection);
        }

        [Theory]
        [InlineData(Direction.Right)]
        [InlineData(Direction.Left)]
        [InlineData(Direction.Up)]
        [InlineData(Direction.Down)]
        [InlineData(Direction.None)]
        public void SnakeShouldBeAbleToChangeIntoEveryDirectionWhenNotMoving(Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.None);

            // Act
            this.testee.UpdateFacingDirection(newDirection);

            // Assert
            this.testee.CurrentDirection.Should().Be(newDirection);
        }

        [Theory]
        [InlineData(Direction.Right)]
        [InlineData(Direction.Left)]
        [InlineData(Direction.Up)]
        [InlineData(Direction.Down)]
        public void SnakeShouldNotBeAbleToChangeIntoNoDirectionWhenAlreadyMoving(Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(newDirection);

            // Act
            this.testee.UpdateFacingDirection(Direction.None);

            // Assert
            this.testee.CurrentDirection.Should().NotBe(Direction.None);
        }

        [Theory]
        [InlineData(Direction.Right)]
        [InlineData(Direction.Left)]
        [InlineData(Direction.Up)]
        [InlineData(Direction.Down)]
        [InlineData(Direction.None)]
        public void WhenNewDirectionIsSameAsOldOneNothingChanges(Direction sameDirection)
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();

            // Act
            this.testee.UpdateFacingDirection(sameDirection);

            // Assert
            this.testee.CurrentDirection.Should().Be(sameDirection);
        }

        [Fact]
        public void WhenSnakeGrowsForTheFirstTimeABodyPartShouldBeAddedAfterTheHeadOfTheSnake()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            Position positionOfHead = this.testee.Head.Position;

            // Act
            this.testee.Grow();

            // Assert
            this.testee.Body.Count.Should().Be(1);
            this.testee.Body.Last().Position.Should().Be(positionOfHead);
            this.testee.HasCollidedWithItSelf.Should().Be(false);
        }

        [Fact]
        public void WhenTheSnakeGrowsABodyPartShouldBeAddedAtTheEndOfTheSnake()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.Grow();
            Position positionOfLastBodyPart = this.testee.Body.Last().Position;

            // Act
            this.testee.Grow();

            // Assert
            this.testee.Body.Count.Should().Be(2);
            this.testee.Body.Last().Position.Should().Be(positionOfLastBodyPart);
            this.testee.HasCollidedWithItSelf.Should().Be(false);
        }

        [Fact]
        public void WhenSnakeHasABodyItMovesWithTheHead()
        {
            // Arrange
            this.testee.ResetSnakeToStartPosition();
            this.testee.UpdateFacingDirection(Direction.Right);
            this.testee.Grow();

            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(0);
            this.testee.Head.Position.Column.Should().Be(1);
            this.testee.Body.Last().Position.Row.Should().Be(0);
            this.testee.Body.Last().Position.Column.Should().Be(0);
            this.testee.HasCollidedWithItSelf.Should().Be(false);
        }
    }
}