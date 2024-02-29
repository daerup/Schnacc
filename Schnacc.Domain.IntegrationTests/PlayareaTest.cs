using FakeItEasy;
using FakeItEasy.Core;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Xbehave;

namespace Schnacc.Domain.IntegrationTests
{
    public class PlayareaTest
    {
        private Playarea.Playarea testee;

        [Background]
        public void SetUp()
        {
            this.testee = null;
        }

        [Scenario]
        private void WhenTheSnakeCollidesInFruitItGrows()
        {
            IFoodFactory foodFactory = null;

            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given the food will pops up in front of snake head"
                .x(() => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored)).Returns(new Apple(new Position(5, 4))).Once());
            "And given a play area with boundaries 10 and 5"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(10, 5), foodFactory));
            "And given the snake at position row 5 and column 2"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 2)));
            "And given the food is in front of the snake"
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
        private void SnakeCannotBeResetUnlessTheGameIsOver()
        {
            IFoodFactory foodFactory = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a play area with size 4 by 4"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(4, 4), foodFactory));
            "And given the snake at position row 2 and column 2"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "And the game state is start"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Start));

            "When the direction of the snake is updated for the first time"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

            "When the snake tries to be reset without being GameOver"
                .x(() => this.testee.RestartGame());
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));

            "when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "When the snake tries to be reset without being GameOver"
                .x(() => this.testee.RestartGame());
            "Then nothing happens"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));

            "When the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 3)));
            "And then the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));

            "When the snake tries to be reset with the game state being GameOVer"
                .x(() => this.testee.RestartGame());
            "Then the snake is reset"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
        }

        [Scenario]
        private void SnakeCannotMoveWhenTheGameIsOver()
        {
            IFoodFactory foodFactory = null;
            "Given a food factory"
                .x(() => foodFactory = A.Dummy<IFoodFactory>());
            "And given a play area"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(4, 5), foodFactory));
            "And given the snake at position row 2 and column 1"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "And the game state is start"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Start));

            "When the direction of the snake is updated for the first time"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));
            "Then the game state should be running"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Running));

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

            "When the snake moves outside of playarea"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 4)));
            "And then the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));

            "When the snake tries to move"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "And when the game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
            "Then the snake should not have moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 4)));
        }

        [Scenario]
        private void GameIsOverWhenSnakeCollidesWithRightWall()
        {
            IFoodFactory foodFactory = null;

            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a play area with boundaries 4 and 5"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(4, 5), foodFactory));
            "And given the snake at position row 2 and column 2"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "And given the snake is facing rightwards"
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
        private void GameIsOverWhenSnakeCollidesWithLeftWall()
        {
            IFoodFactory foodFactory = null;

            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a play area with boundaries 4 and 4"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(4, 4), foodFactory));
            "And given the snake at position row 2 and column 2"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "And given the snake is facing leftwards"
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
        private void GameIsOverWhenSnakeCollidesWithTopWall()
        {
            IFoodFactory foodFactory = null;

            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a play area with boundaries 4 and 4"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(4, 4), foodFactory));
            "And given the snake at position row 2 and column 2"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "And given the snake is facing upwards"
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
        private void GameIsOverWhenSnakeCollidesWithBottomWall()
        {
            IFoodFactory foodFactory = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given a play area with boundaries 5 and 5"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(5, 5), foodFactory));
            "And given the snake at position row 2 and column 2"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(2, 2)));
            "And given the snake is facing downwards"
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
        private void GameIsOverWhenSnakeCollidesWithItself()
        {
            IFoodFactory foodFactory = null;
            "Given a food factory"
                .x(() => foodFactory = A.Fake<IFoodFactory>());
            "And given the food will pops up in front of snake head"
                .x(() => A.CallTo(() => foodFactory.CreateRandomFoodBetweenBoundaries(A<Position>.Ignored))
                          .ReturnsLazily(this.CalculateNewFoodPosition));
            "And given a play area with boundaries 10 and 12"
                .x(() => this.testee = new Playarea.Playarea(new PlayareaSize(10, 12), foodFactory));
            "And given the snake at position row 5 and column 6"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 6)));
            "And given the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 7)));
            "And the snake is facing rightwards"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Right));


            "When the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 7)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(1));

            "When the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 8)));
            "And the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 8)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(2));

            "When the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 9)));
            "When the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 9)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(3));

            "When the food is actually in front of the snake"
                .x(() => this.testee.Food.Position.Should().BeEquivalentTo(new Position(5, 10)));
            "When the snake moves into the food"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(5, 10)));
            "and then the snake has grown"
                .x(() => this.testee.Snake.Body.Count.Should().Be(4));

            "When the snake changes direction by 90 degrees"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Up));
            "And when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 10)));

            "When the snake changes direction by 90 degrees"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Left));
            "And when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake has moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 9)));

            "When the snake changes direction by 90 degrees"
                .x(() => this.testee.UpdateSnakeDirection(Direction.Down));
            "And when the snake moves"
                .x(() => this.testee.MoveSnakeWhenAllowed());
            "Then the snake should have collided with itself and the Game is over"
                .x(() => this.testee.CurrentGameState.Should().Be(Game.Over));
            "Then the snake has not moved"
                .x(() => this.testee.Snake.Head.Position.Should().BeEquivalentTo(new Position(4, 9)));
        }

        private Food.Food CalculateNewFoodPosition(IFakeObjectCall arg) => this.testee != null ? new Apple(new Position(5, this.testee.Snake.Head.Position.Column + 1)) : new Apple(new Position(5, 7));
    }
}