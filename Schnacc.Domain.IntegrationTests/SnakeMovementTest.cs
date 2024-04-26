using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Schnacc.Domain.Snake;
using Xbehave;

namespace Schnacc.Domain.IntegrationTests
{
    [SuppressMessage("ReSharper", "TooManyDeclarations")]
    public class SnakeMovementTest
    {
        private Snake.Snake testee;

        private void GivingASnakeWithABodyFacingInACertainDirection(int startRow, int startColumn, Direction certainDirection)
        {
            "Given a snake"
                .x(() => this.testee = new Snake.Snake(new Position(startRow, startColumn)));
            "And the snake is facing right"
                .x(() => this.testee.UpdateFacingDirection(certainDirection));
            "And the snake has a body containing one body part"
                .x(() => this.testee.Grow());
        }

        private void WhenASnakeGrowsASnakeBodyPartShouldBeAdded()
        {
            int previousCount = 0;
            "Given the previous body length"
                .x(() => previousCount = this.testee.Body.Count);
            "When the snake grows due to fruit consumption"
                .x(() => this.testee.Grow());
            "Then the snake body should contain 2 body parts"
                .x(() => this.testee.Body.Count.Should().Be(previousCount + 1));
            "And then the snake has not collided with it self yet"
                .x(() => this.testee.HasCollidedWithItSelf.Should().Be(false));
        }

        [Scenario]
        [Example(0, 0)]
        private void BodyOfSnakeIsFollowingTheSnakeHeadRightwards(int startRow, int startColumn)
        {
            this.GivingASnakeWithABodyFacingInACertainDirection(startRow, startColumn, Direction.Right);

            "When the snake moves"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the body of the snake has not moved yet"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 0 Rows and 2 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 2)));
            "And then the first body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the second body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 0 Rows and 3 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 3)));
            "And then the first body part of the snake has moved 0 Rows and 2 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 2)));
            "And then the second body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the third body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[2].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
        }

        [Scenario]
        [Example(0, 0)]
        private void BodyOfSnakeIsFollowingTheSnakeHeadLeftwards(int startRow, int startColumn)
        {
            this.GivingASnakeWithABodyFacingInACertainDirection(startRow, startColumn, Direction.Left);

            "When the snake moves"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn - 1)));
            "And then the body of the snake has not moved yet"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 0 Rows and 2 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn - 2)));
            "And then the first body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn - 1)));
            "And then the second body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 0 Rows and 3 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn - 3)));
            "And then the first body part of the snake has moved 0 Rows and 2 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn - 2)));
            "And then the second body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn - 1)));
            "And then the third body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[2].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
        }

        [Scenario]
        [Example(0, 0)]
        private void BodyOfSnakeIsFollowingTheSnakeHeadUpwards(int startRow, int startColumn)
        {
            this.GivingASnakeWithABodyFacingInACertainDirection(startRow, startColumn, Direction.Up);

            "When the snake moves"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow - 1, startColumn)));
            "And then the body of the snake has not moved yet"
                .x(() => this.testee.Body.First().Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 2 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow - 2, startColumn)));
            "And then the first body part of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow - 1, startColumn)));
            "And then the second body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 3 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow - 3, startColumn)));
            "And then the first body part of the snake has moved 2 Rows and 0 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow - 2, startColumn)));
            "And then the second body part of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow - 1, startColumn)));
            "And then the third part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[2].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
        }

        [Scenario]
        [Example(0, 0)]
        private void BodyOfSnakeIsFollowingTheSnakeHeadDownwards(int startRow, int startColumn)
        {
            this.GivingASnakeWithABodyFacingInACertainDirection(startRow, startColumn, Direction.Down);

            "When the snake moves"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn)));
            "And then the body of the snake has not moved yet"
                .x(() => this.testee.Body.First().Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 2 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow + 2, startColumn)));
            "And then the first body part of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn)));
            "And then the second body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 3 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow + 3, startColumn)));
            "And then the first body part of the snake has moved 2 Rows and 0 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow + 2, startColumn)));
            "And then the second body part of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn)));
            "And then the third body part of the snake has moved 0 Rows and 0 Columns"
                .x(() => this.testee.Body[2].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
        }

        [Scenario]
        [Example(0, 0)]
        private void BodyOfSnakeIsCapableOfMovingInCircularMotions(int startRow, int startColumn)
        {
            this.GivingASnakeWithABodyFacingInACertainDirection(startRow, startColumn, Direction.Right);

            "When the snake moves"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and 0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the body of the snake has not moved yet"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
            "And then the snake has not collided with it self yet"
                .x(() => this.testee.HasCollidedWithItSelf.Should().Be(false));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake turns by 90 degrees"
                .x(() => this.testee.UpdateFacingDirection(Direction.Down));
            "And the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and 1 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn + 1)));
            "And then the first body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the second body part of the snake has not moved yet"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
            "And then the snake has not collided with it self yet"
                .x(() => this.testee.HasCollidedWithItSelf.Should().Be(false));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake turns by 90 degrees"
                .x(() => this.testee.UpdateFacingDirection(Direction.Left));
            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and  0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn)));
            "And then the first body part of the snake has moved 1 Rows and 1 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn + 1)));
            "And then the second body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the third body part of the snake has not moved yet"
                .x(() => this.testee.Body[2].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
            "And then the snake has not collided with it self yet"
                .x(() => this.testee.HasCollidedWithItSelf.Should().Be(false));

            this.WhenASnakeGrowsASnakeBodyPartShouldBeAdded();

            "When the snake turns by 90 degrees"
                .x(() => this.testee.UpdateFacingDirection(Direction.Up));
            "When the snake moves again"
                .x(() => this.testee.Move());
            "Then the head of the snake has moved 1 Rows and  0 Columns"
                .x(() => this.testee.Head.Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
            "And then the first body part of the snake has moved 1 Rows and 1 Columns"
                .x(() => this.testee.Body[0].Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn)));
            "And then the second body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[1].Position.Should().BeEquivalentTo(new Position(startRow + 1, startColumn + 1)));
            "And then the third body part of the snake has moved 0 Rows and 1 Columns"
                .x(() => this.testee.Body[2].Position.Should().BeEquivalentTo(new Position(startRow, startColumn + 1)));
            "And then the forth body part of the snake has not moved yet"
                .x(() => this.testee.Body[3].Position.Should().BeEquivalentTo(new Position(startRow, startColumn)));
            "And then the snake has collided with it self"
                .x(() => this.testee.HasCollidedWithItSelf.Should().Be(true));
        }
    }
}