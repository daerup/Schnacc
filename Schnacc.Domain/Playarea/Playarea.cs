using System.Collections.Generic;
using System.Linq;
using Schnacc.Domain.Food;
using Schnacc.Domain.Snake;

namespace Schnacc.Domain.Playarea
{
    public class Playarea
    {
        public readonly PlayareaSize Size;

        private readonly IFoodFactory factory;

        private readonly Snake.Snake snake;

        public Playarea(PlayareaSize size, IFoodFactory factory)
        {
            this.Size = this.GetValidFieldSize(size);
            this.factory = factory;
            var startPosition = new Position(this.Size.NumberOfRows / 2, this.Size.NumberOfColumns / 2);
            this.snake = new Snake.Snake(startPosition);
            this.Food = this.GetRandomFoodInUniquePosition();
            this.SetGameState();
        }

        public IFood Food { get; private set; }

        public ISnake Snake => this.snake;

        public Game CurrentGameState { get; private set; }

        private bool SnakeCollidedWithFood => this.Food.Position.Equals(this.snake.Head.Position);

        public void MoveSnakeWhenAllowed()
        {
            this.SetGameState();
            if (this.CurrentGameState.Equals(Game.Over))
            {
                return;
            }

            this.snake.Move();

            if (this.SnakeCollidedWithFood)
            {
                this.snake.Grow();
                this.Food = this.GetRandomFoodInUniquePosition();
            }
        }

        public void UpdateSnakeDirection(Direction newDirection)
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                return;
            }

            this.snake.UpdateFacingDirection(newDirection);
            this.SetGameState();
        }

        public void RestartGame()
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                this.snake.ResetSnakeToStartPosition();
            }
        }

        private void SetGameState()
        {
            if (this.snake.CurrentDirection.Equals(Direction.None))
            {
                this.CurrentGameState = Game.Start;
                return;
            }

            if (this.NextPositionIsValid() == false)
            {
                this.CurrentGameState = Game.Over;
                return;
            }

            this.CurrentGameState = Game.Running;
        }

        private bool NextPositionIsValid() => !this.NextPositionCollidesWithSnakeBody() && !this.NextPositionCollidesWithWalls();

        private bool NextPositionCollidesWithSnakeBody()
        {
            Position nextHeadPosition = this.snake.GetNextHeadPosition();
            return this.snake.Body.Select(sb => sb.Position).Any(p => p.Equals(nextHeadPosition));
        }

        private bool NextPositionCollidesWithWalls()
        {
            Position nextPosition = this.snake.GetNextHeadPosition();
            return
                nextPosition.Column <= -1 ||
                nextPosition.Row <= -1 ||
                nextPosition.Column == this.GetCorner().Column ||
                nextPosition.Row == this.GetCorner().Row;
        }

        private PlayareaSize GetValidFieldSize(PlayareaSize size)
        {
            int row = size.NumberOfRows;
            int column = size.NumberOfColumns;
            if (size.NumberOfRows < 4)
            {
                row = 4;
            }

            if (size.NumberOfColumns < 4)
            {
                column = 4;
            }

            return new PlayareaSize(row, column);
        }

        private Position GetCorner() => new Position(this.Size.NumberOfRows, this.Size.NumberOfColumns);

        private IFood GetRandomFoodInUniquePosition()
        {
            List<Position> allUsedPositions = this.snake.Body.Select(bp => bp.Position).ToList();
            allUsedPositions.Add(this.snake.Head.Position);
            IFood randomFood;

            do
            {
                randomFood = this.factory.CreateRandomFoodBetweenBoundaries(this.GetCorner());
            }
            while (allUsedPositions.Any(up => up.Equals(randomFood.Position)));

            return randomFood;
        }
    }
}