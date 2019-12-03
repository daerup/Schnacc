namespace Schnacc.Domain.IntegrationTests
{
    using FakeItEasy;
    using FluentAssertions;
    using Food;
    using Playarea;
    using Snake;
    using Xbehave;
    using Schnacc.Domain;

    public class PlayareaTest
    {
        private Playarea testee;

        [Scenario]
        private void whenTheSnakeCollidesInFruitItGrows()
        {
            FoodFactory foodFactory = null;
            "Given a food factory with 10 x 10 boundaries"
                .x(() => foodFactory = A.Fake<FoodFactory>((x => x.WithArgumentsForConstructor(() => new FoodFactory(new Position(10,10))))));
            "And given the food pops up in front of snake head"
                .x(() => A.CallTo(() => foodFactory.CreateRandomFood()).Returns(new Apple(new Position(5, 4))));
            "And given a play area"
                .x(() => this.testee = new Playarea(foodFactory));
            "And the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "When the snake moves into an empty place, it doesn't grows"
                .x(() => this.testee.MoveSnake());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 3)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(0));

            "When the snake moves into the food, it grows"
                .x(() => this.testee.MoveSnake());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(1));
        }

        [Scenario]
        private void snakeCannotBeResetUnlessTheGameIsOver()
        {
            "Given a play area"
                .x(() => this.testee = new Playarea(new FoodFactory(new Position(4, 3))));
            "And the game state is start"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Start));

            "When the direction of the snake is updated for the first time"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake tries to be reset without being GameOVer"
                .x(() => this.testee.RestartGame());
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));

            "when the snake moves"
                .x(() => this.testee.MoveSnake());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));

            "and then the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));

            "When the snake tries to be reset with the game state being GameOVer"
                .x(() => this.testee.RestartGame());
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
        }
    }
}