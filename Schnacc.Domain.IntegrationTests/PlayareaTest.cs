﻿namespace Schnacc.Domain.IntegrationTests
{
    using FakeItEasy;

    using FluentAssertions;

    using Food;

    using Playarea;

    using Schnacc.Domain;

    using Snake;

    using Xbehave;

    public class PlayareaTest
    {
        private Playarea testee;

        [Scenario]
        private void whenTheSnakeCollidesInFruitItGrows()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 5 and column 2"
                .x(() => snake = new Snake(new Position(5, 2)));
            "And given the food will pops up in front of snake head"
                .x(body: () => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).Returns(new Apple(new Position(5, 4))));
            "And given a play area with boundaries 10 and 10"
                .x(() => this.testee = new Playarea(new Position(10, 10), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "When the snake moves into an empty place, it doesn't grows"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 3)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(0));

            "When the snake moves into the food, it grows"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(1));
        }

        [Scenario]
        private void snakeCannotBeResetUnlessTheGameIsOver()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 2 and column 2"
                .x(() => snake = new Snake(new Position(2, 2)));
            "And given a play area"
                .x(() => this.testee = new Playarea(new Position(4, 4), foodFactory, snake));
            "And the game state is start"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Start));

            "When the direction of the snake is updated for the first time"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake tries to be reset without being GameOver"
                .x(() => this.testee.RestartGame(new Position(1, 1)));
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));

            "when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "When the snake tries to be reset without being GameOVer"
                .x(() => this.testee.RestartGame(new Position(1, 1)));
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));

            "when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 4)));

            "and then the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));

            "When the snake tries to be reset with the game state being GameOVer"
                .x(() => this.testee.RestartGame(new Position(1, 1)));
            "Then the snake is reset"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(1, 1)));
        }
    }
}