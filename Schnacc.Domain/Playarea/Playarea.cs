namespace Schnacc.Domain.Playarea
{
    using System.Collections.Generic;
    using System.Linq;

    using Food;

    using Snake;

    public class Playarea
    {
        public readonly Position WallCorner;

        private readonly IFoodFactory factory;

        private readonly Snake snake;

        public Playarea(PlayareaSize size, IFoodFactory factory, Snake snake)
        {
            this.WallCorner = this.getValidWallCorner(size);
            this.factory = factory;
            this.snake = snake;
            this.Food = this.getRandomFoodInUniquePosition();
        }

        public IFood Food { get; private set; }

        public ISnake Snake => this.snake;

        public Game CurrentGameState
        {
            get
            {
                if (this.snake.CurrentDirection.Equals(Direction.None))
                {
                    return Game.Start;
                }

                if (this.SnakeCollidedWithWalls || this.snake.HasCollidedWithItSelf)
                {
                    return Game.Over;
                }

                return Game.Running;
            }
        }

        private bool SnakeCollidedWithWalls =>
            this.snake.Head.Position.Column == 0 || 
            this.snake.Head.Position.Row == 0 ||
            this.snake.Head.Position.Column == this.WallCorner.Column ||
            this.snake.Head.Position.Row == this.WallCorner.Row;

        private bool SnakeCollidedWithFood => this.Food.Position.Equals(this.snake.Head.Position);

        public void MoveSnakeWhenAllowed()
        {
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
            }
        }

        public void RestartGame(Position snakeStartPosition)
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                this.snake.ResetSnakeToPosition(snakeStartPosition);
            }
        }

        private Position getValidWallCorner(PlayareaSize size)
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

            return new Position(row + 1, column + 1);
        }

        private IFood getRandomFoodInUniquePosition()
        {
            List<Position> allUsedPositions = this.snake.Body.Select(bp => bp.Position).ToList();
            allUsedPositions.Add(this.snake.Head.Position);
            IFood randomFood;

            do
            {
                randomFood = this.factory.CreateRandomFoodBetweenBoundaries(this.WallCorner);
            }
            while (allUsedPositions.Contains(randomFood.Position));

            return randomFood;
        }
    }
}