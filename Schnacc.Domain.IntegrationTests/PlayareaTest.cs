namespace Schnacc.Domain.IntegrationTests
{
    using FakeItEasy;

    using FluentAssertions;

    using Food;

    using Playarea;

    using Snake;

    using Xbehave;

    public class PlayareaTest
    {
        private Playarea testee;

        [Scenario]
        public void whenTheSnakeCollidesInFruitItGrows()
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
                .x(() => this.testee = new Playarea(new PlayareaSize(10, 10), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            
            "When the snake moves into an empty place"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 3)));
            "and then the snake has not grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(0));

            "When the snake moves into the food"
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
            "And given a play area with size 4 by 4"
                .x(() => this.testee = new Playarea(new PlayareaSize(4, 4), foodFactory, snake));
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
            "When the snake tries to be reset without being GameOver"
                .x(() => this.testee.RestartGame(new Position(1, 1)));
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "And then the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));

            "When the snake tries to be reset with the game state being GameOVer"
                .x(() => this.testee.RestartGame(new Position(1, 1)));
            "Then the snake is reset"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(1, 1)));
        }

        [Scenario]
        private void snakeCannotMoveWhenTheGameIsOver()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Dummy<IFoodFactory>());
            "And given a snake at position row 2 and column 2"
                .x(() => snake = new Snake(new Position(2, 1)));
            "And given a play area"
                .x(() => this.testee = new Playarea(new PlayareaSize(4, 4), foodFactory, snake));
            "And the game state is start"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Start));

            "When the direction of the snake is updated for the first time"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves outside of playarea"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "And then the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));

            "When the snake tries to move"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "And when the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
        }

        [Scenario]
        public void gameIsOverWhenSnakeCollidesWithRightWall()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 5 and column 2"
                .x(() => snake = new Snake(new Position(2, 2)));
            "And given the food will pops up in front of snake head"
                .x(body: () => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).Returns(new Apple(new Position(5, 4))));
            "And given a play area with boundaries 10 and 10"
                .x(() => this.testee = new Playarea(new PlayareaSize(5, 5), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And given the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            
            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 4)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves into wall"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 4)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
        }

        [Scenario]
        public void gameIsOverWhenSnakeCollidesWithRLeftWall()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 5 and column 2"
                .x(() => snake = new Snake(new Position(2, 2)));
            "And given the food will pops up in front of snake head"
                .x(body: () => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).Returns(new Apple(new Position(5, 4))));
            "And given a play area with boundaries 10 and 10"
                .x(() => this.testee = new Playarea(new PlayareaSize(5, 5), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And given the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Left));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 1)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 0)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves into wall"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 0)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
        }

        [Scenario]
        public void gameIsOverWhenSnakeCollidesWithTopWall()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 5 and column 2"
                .x(() => snake = new Snake(new Position(2, 2)));
            "And given the food will pops up in front of snake head"
                .x(body: () => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).Returns(new Apple(new Position(5, 4))));
            "And given a play area with boundaries 10 and 10"
                .x(() => this.testee = new Playarea(new PlayareaSize(5, 5), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And given the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Up));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(1, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(0, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves into wall"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(0, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
        }

        [Scenario]
        public void gameIsOverWhenSnakeCollidesWithBottomWall()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 5 and column 2"
                .x(() => snake = new Snake(new Position(2, 2)));
            "And given the food will pops up in front of snake head"
                .x(body: () => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).Returns(new Apple(new Position(5, 4))));
            "And given a play area with boundaries 10 and 10"
                .x(() => this.testee = new Playarea(new PlayareaSize(5, 5), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And given the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Down));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(3, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake moves into wall"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 2)));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
        }

        [Scenario]
        public void gameIsOverWhenSnakeCollidesWithItself()
        {
            IFoodFactory foodFactory = null;
            Snake snake = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a snake at position row 5 and column 2"
                .x(() => snake = new Snake(new Position(5, 2)));
            "And given the food will pops up in front of snake head"
                .x(body: () => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).ReturnsLazily(() => new Apple(new Position(5, snake.Head.Position.Column + 1))));
            "And given a play area with boundaries 10 and 10"
                .x(() => this.testee = new Playarea(new PlayareaSize(10, 10), foodFactory, snake));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 3)));
            "And the snake is facing right wards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));


            "When the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 3)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(1));

            "When the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "And the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 4)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(2));

            "When the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 5)));
            "When the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 5)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(3));

            "When the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 6)));
            "When the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 6)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(4));

            "When the snake changes direction by 90 degrees"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Up));
            "And when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 6)));

            "When the snake changes direction by 90 degrees"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Left));
            "And when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 5)));

            "When the snake changes direction by 90 degrees"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Down));
            "And when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have collided with itself and the Game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
            "Then the snake has not moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 5)));
        }
    }
}