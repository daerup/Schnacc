using System.Linq;
using FluentAssertions;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Snake
{
    public class SnakeTest
    {
        private readonly Domain.Snake.Snake _testee = new Domain.Snake.Snake(new Position(0, 0));

        [Fact]
        public void ForwardsFacingSnakeShouldMoveFarwards()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.Right);
            
            // Act
            this._testee.Move();

            // Assert
            this._testee.Head.Position.Row.Should().Be(0);
            this._testee.Head.Position.Column.Should().Be(1);
        }

        [Fact]
        public void BackwardsFacingSnakeShouldMoveBackwards()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.Left);
            
            // Act
            this._testee.Move();

            // Assert
            this._testee.Head.Position.Row.Should().Be(0);
            this._testee.Head.Position.Column.Should().Be(-1);
        }

        [Fact]
        public void UpwardsFacingSnakeShouldMoveUpwards()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.Up);
            
            // Act
            this._testee.Move();

            // Assert
            this._testee.Head.Position.Row.Should().Be(-1);
            this._testee.Head.Position.Column.Should().Be(0);
        }

        [Fact]
        public void DownwardsFacingSnakeShouldMoveDownwards()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.Down);
            
            // Act
            this._testee.Move();

            // Assert
            this._testee.Head.Position.Row.Should().Be(1);
            this._testee.Head.Position.Column.Should().Be(0);
        }

        [Fact]
        public void StillStandingSnakeShouldNotMove()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.None);
            
            // Act
            this._testee.Move();

            // Assert
            this._testee.Head.Position.Row.Should().Be(0);
            this._testee.Head.Position.Column.Should().Be(0);
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
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(startDirection);

            // Act
            this._testee.UpdateFacingDirection(newDirection);

            // Assert
            this._testee.CurrentDirection.Should().Be(newDirection);
        }

        [Theory]
        [InlineData(Direction.Right, Direction.Left)]
        [InlineData(Direction.Left, Direction.Right)]
        [InlineData(Direction.Up, Direction.Down)]
        [InlineData(Direction.Down, Direction.Up)]
        public void SnakeShouldNotBeAbleToChangeDirectionBy180Degrees(Direction startDirection, Direction newDirection)
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(startDirection);

            // Act
            this._testee.UpdateFacingDirection(newDirection);

            // Assert
            this._testee.CurrentDirection.Should().NotBe(newDirection);
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
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.None);

            // Act
            this._testee.UpdateFacingDirection(newDirection);

            // Assert
            this._testee.CurrentDirection.Should().Be(newDirection);
        }

        [Theory]
        [InlineData(Direction.Right)]
        [InlineData(Direction.Left)]
        [InlineData(Direction.Up)]
        [InlineData(Direction.Down)]
        public void SnakeShouldNotBeAbleToChangeIntoNoDirectionWhenAlreadyMoving(Direction newDirection)
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(newDirection);

            // Act
            this._testee.UpdateFacingDirection(Direction.None);

            // Assert
            this._testee.CurrentDirection.Should().NotBe(Direction.None);
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
            this._testee.ResetSnakeToStartPosition();

            // Act
            this._testee.UpdateFacingDirection(sameDirection);

            // Assert
            this._testee.CurrentDirection.Should().Be(sameDirection);
        }

        [Fact]
        public void WhenSnakeGrowsForTheFirstTimeABodyPartShouldBeAddedAfterTheHeadOfTheSnake()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            var positionOfHead = this._testee.Head.Position;

            // Act
            this._testee.Grow();

            // Assert
            this._testee.Body.Count.Should().Be(1);
            this._testee.Body.Last().Position.Should().Be(positionOfHead);
            this._testee.HasCollidedWithItSelf.Should().Be(false);
        }

        [Fact]
        public void WhenTheSnakeGrowsABodyPartShouldBeAddedAtTheEndOfTheSnake()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.Grow();
            var positionOfLastBodyPart = this._testee.Body.Last().Position;

            // Act
            this._testee.Grow();

            // Assert
            this._testee.Body.Count.Should().Be(2);
            this._testee.Body.Last().Position.Should().Be(positionOfLastBodyPart);
            this._testee.HasCollidedWithItSelf.Should().Be(false);
        }

        [Fact]
        public void WhenSnakeHasABodyItMovesWithTheHead()
        {
            // Arrange
            this._testee.ResetSnakeToStartPosition();
            this._testee.UpdateFacingDirection(Direction.Right);
            this._testee.Grow();

            // Act
            this._testee.Move();

            // Assert
            this._testee.Head.Position.Row.Should().Be(0);
            this._testee.Head.Position.Column.Should().Be(1);
            this._testee.Body.Last().Position.Row.Should().Be(0);
            this._testee.Body.Last().Position.Column.Should().Be(0);
            this._testee.HasCollidedWithItSelf.Should().Be(false);
        }
    }
}