namespace Schnacc.Domain.Playarea
{
    using System.Collections.Generic;
    using Food;
    using Snake;
    using System.Linq;

    public class Playarea
    {
        private readonly Snake snake;
        private readonly FoodFactory factory;

        private IFood food;


        public Playarea(FoodFactory factory)
        {
            this.factory = factory;
            this.snake = new Snake(new Position(factory.Boundaries.maxRow / 2, 2));
            this.food = this.getRandomFoodInUniquePosition();
        }

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

        public void MoveSnake()
        {
            if (this.CurrentGameState.Equals(Game.Over) == false)
            {
                this.snake.Move();
            }

            if (this.SnakeCollidedWithFood)
            {
                this.snake.Grow();
                this.food = this.getRandomFoodInUniquePosition();
            }
        }

        public void UpdateSnakeDirection(Direction newDirection)
        {
            if (this.CurrentGameState.Equals(Game.Over) == false)
            {
                this.snake.UpdateFacingDirection(newDirection);
            }
        }

        public void RestartGame()
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                this.snake.ResetSnakeToPosition(new Position(this.factory.Boundaries.maxRow / 2, 2));
            }
        }

        public ISnake Snake => this.snake;

        private bool SnakeCollidedWithWalls =>
            this.snake.Head.Position.Column == 0 || 
            this.snake.Head.Position.Row == 0 ||
            this.snake.Head.Position.Column == this.factory.Boundaries.maxColumn ||
            this.snake.Head.Position.Row == this.factory.Boundaries.maxRow;

        private bool SnakeCollidedWithFood => this.food.Position.Equals(this.snake.Head.Position);

        private IFood getRandomFoodInUniquePosition()
        {
            List<Position> allUsedPositions = this.snake.Body.Select(bp => bp.Position).ToList();
            allUsedPositions.Add(this.snake.Head.Position);
            IFood randomFood;

            do
            {
                randomFood = this.factory.CreateRandomFood();
            } while (allUsedPositions.Contains(randomFood.Position));

            return randomFood;
        }
    }
}