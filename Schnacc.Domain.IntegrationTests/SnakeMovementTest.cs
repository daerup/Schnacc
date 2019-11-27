namespace Schnacc.Domain.IntegrationTests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using FluentAssertions;
    using Schnacc.Domain.Snake;
    using Xbehave;

    [SuppressMessage("ReSharper", "TooManyDeclarations")]
    public class SnakeMovementTest
    {
        private Snake testee;

        [Background]
        public void Background()
        {
            "Given a snake"
                .x(() => this.testee = new Snake(0, 0));
        }

        // TODO replace Grow() with fake bodypart list 
        [Scenario]
        [Example(0, 0)]
        private void bodyOfSnakeIsFollowingHeadWhenMoving(int startRow, int startColumn)
        {
            "And the snake is facing right"
                .x(() => this.testee.UpdateFacingDirection(Direction.Right));
            "And the snake has a body containing one body part"
                .x(() => this.testee.Grow());

            "When the snake moves"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 columns"
                .x(() => this.testee.Head.Position.column.Should().Be(startColumn + 1));
            "And then the head of the snake has moved 0 rows"
                .x(() => this.testee.Head.Position.row.Should().Be(startRow));
            "And then the body of the snake has not moved yet"
                .x(() => this.testee.Body.First().Position.Should().Be((startColumn, startRow)));

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 2 columns"
                .x(() => this.testee.Head.Position.column.Should().Be(startColumn + 2));
            "And then the head of the snake has moved 0 rows"
                .x(() => this.testee.Head.Position.row.Should().Be(startRow));
            "And then the body of the snake has moved 1 columns"
                .x(() => this.testee.Body.First().Position.column.Should().Be(startColumn + 1));
            "And then the body of the snake has moved 0 rows"
                .x(() => this.testee.Body.First().Position.column.Should().Be(startRow));
        }
    }
}
