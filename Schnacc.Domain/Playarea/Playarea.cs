namespace Schnacc.Domain.Playarea
{
    using System.Collections.Generic;
    using System.Linq;

    using Food;

    using Snake;

    public class Playarea
    {
        public readonly PlayareaSize Size;

        private readonly IFoodFactory factory;

        private readonly Snake snake;

        public Playarea(PlayareaSize size, IFoodFactory factory, Snake snake)
        {
            this.Size = this.getValidFieldSize(size);
            this.factory = factory;
            this.snake = snake;
            this.Food = this.getRandomFoodInUniquePosition();
            this.setGameState();
        }

        public IFood Food { get; private set; }

        public ISnake Snake => this.snake;

        public Game CurrentGameState { get; private set; }

        private bool SnakeCollidedWithFood => this.Food.Position.Equals(this.snake.Head.Position);

        public void MoveSnakeWhenAllowed()
        {
            this.setGameState();
            if (this.CurrentGameState.Equals(Game.Over))
            {
                return;
            }

            this.snake.Move();

            if (this.SnakeCollidedWithFood)
            {
                this.snake.Grow();
                this.Food = this.getRandomFoodInUniquePosition();
            }
        }

        public void UpdateSnakeDirection(Direction newDirection)
        {
            if (this.CurrentGameState.Equals(Game.Over) == false)
            {
                this.snake.UpdateFacingDirection(newDirection);
                this.setGameState();
            }
        }

        public void RestartGame(Position snakeStartPosition)
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                this.snake.ResetSnakeToPosition(snakeStartPosition);
            }
        }

        private void setGameState()
        {
            if (this.snake.CurrentDirection.Equals(Direction.None))
            {
                this.CurrentGameState = Game.Start;
                return;
            }

            if (this.nextPositionIsValid() == false)
            {
                this.CurrentGameState = Game.Over;
                return;
            }

            this.CurrentGameState = Game.Running;
        }

        private bool nextPositionIsValid()
        {
            return !this.nextPositionCollidesWithSnakeBody() && !this.nextPositionCollidesWithWalls();
        }

        private bool nextPositionCollidesWithSnakeBody()
        {
            Position nextHeadPosition = this.snake.GetNextHeadPosition();
            return this.snake.Body.Select(sb => sb.Position).Any(p => p.Equals(nextHeadPosition));
        }

        private bool nextPositionCollidesWithWalls()
        {
            Position nextPosition = this.snake.GetNextHeadPosition();
            return
                nextPosition.Column == -1 ||
                nextPosition.Row == -1 ||
                nextPosition.Column == this.getCorner().Column ||
                nextPosition.Row == this.getCorner().Row;
        }

        private PlayareaSize getValidFieldSize(PlayareaSize size)
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

        private Position getCorner() => new Position(this.Size.NumberOfRows, this.Size.NumberOfColumns);

        private IFood getRandomFoodInUniquePosition()
        {
            List<Position> allUsedPositions = this.snake.Body.Select(bp => bp.Position).ToList();
            allUsedPositions.Add(this.snake.Head.Position);
            IFood randomFood;

            do
            {
                randomFood = this.factory.CreateRandomFoodBetweenBoundaries(this.getCorner());
            }
            while (allUsedPositions.Contains(randomFood.Position));

            return randomFood;
        }
    }
}