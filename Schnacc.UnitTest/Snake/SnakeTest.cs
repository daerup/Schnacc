﻿namespace Schnacc.Domain.UnitTests.Snake
{
    using System.Linq;

    using FluentAssertions;

    using Schnacc.Domain.Snake;

    using Xunit;

    public class SnakeTest
    {
        private readonly Snake testee = new Snake(new Position(0, 0));

        [Fact]
        private void forwardsFacingSnakeShouldMoveFarwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
            this.testee.UpdateFacingDirection(Direction.Right);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(0);
            this.testee.Head.Position.Column.Should().Be(1);
        }

        [Fact]
        private void backwardsFacingSnakeShouldMoveBackwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
            this.testee.UpdateFacingDirection(Direction.Left);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(0);
            this.testee.Head.Position.Column.Should().Be(-1);
        }

        [Fact]
        private void upwardsFacingSnakeShouldMoveUpwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
            this.testee.UpdateFacingDirection(Direction.Up);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(-1);
            this.testee.Head.Position.Column.Should().Be(0);
        }

        [Fact]
        private void downwardsFacingSnakeShouldMoveDownwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
            this.testee.UpdateFacingDirection(Direction.Down);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.Row.Should().Be(1);
            this.testee.Head.Position.Column.Should().Be(0);
        }

        [Fact]
        private void stillStandingSnakeShouldNotMove()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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
        private void snakeShouldBeAbleToChangeDirectionBy90Degrees(Direction startDirection, Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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
        private void snakeShouldNotBeAbleToChangeDirectionBy180Degrees(Direction startDirection, Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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
        private void snakeShouldBeAbleToChangeIntoEveryDirectionWhenNotMoving(Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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
        private void snakeShouldNotBeAbleToChangeIntoNoDirectionWhenAlreadyMoving(Direction newDirection)
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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
        private void whenNewDirectionIsSameAsOldOneNothingChanges(Direction sameDirection)
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));

            // Act
            this.testee.UpdateFacingDirection(sameDirection);

            // Assert
            this.testee.CurrentDirection.Should().Be(sameDirection);
        }

        [Fact]
        private void whenSnakeGrowsForTheFirstTimeABodyPartShouldBeAddedAfterTheHeadOfTheSnake()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
            Position positionOfHead = this.testee.Head.Position;

            // Act
            this.testee.Grow();

            // Assert
            this.testee.Body.Count.Should().Be(1);
            this.testee.Body.Last().Position.Should().Be(positionOfHead);
            this.testee.HasCollidedWithItSelf.Should().Be(false);
        }

        [Fact]
        private void whenTheSnakeGrowsABodyPartShouldBeAddedAtTheEndOfTheSnake()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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
        private void whenSnakeHasABodyItMovesWithTheHead()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(new Position(0, 0));
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