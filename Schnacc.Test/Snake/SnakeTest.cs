using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schnacc.Domain.Test.Snake
{
    using FluentAssertions;

    using Schnacc.Domain.Snake;
    using Schnacc.Domain.Snake.Orientation;

    using Xunit;
    using Xunit.Sdk;

    public class SnakeTest
    {
        private readonly Snake testee = new Snake(0, 0);


        [Fact]
        private void forwardsFacingSnakeShouldMoveFarwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(0, 0);
            this.testee.UpdateOrientation(OrientationDirection.Forwards);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.row.Should().Be(0);
            this.testee.Head.Position.column.Should().Be(1);
        }

        [Fact]
        private void backwardsFacingSnakeShouldMoveBackwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(0, 0);
            this.testee.UpdateOrientation(OrientationDirection.Backwards);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.row.Should().Be(0);
            this.testee.Head.Position.column.Should().Be(-1);
        }

        [Fact]
        private void upwardsFacingSnakeShouldMoveUpwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(0, 0);
            this.testee.UpdateOrientation(OrientationDirection.Upwards);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.row.Should().Be(-1);
            this.testee.Head.Position.column.Should().Be(0);
        }

        [Fact]
        private void downwardsFacingSnakeShouldMoveDownwards()
        {
            // Arrange
            this.testee.ResetSnakeToPosition(0, 0);
            this.testee.UpdateOrientation(OrientationDirection.Downwards);
            
            // Act
            this.testee.Move();

            // Assert
            this.testee.Head.Position.row.Should().Be(1);
            this.testee.Head.Position.column.Should().Be(0);
        }
    }
}
