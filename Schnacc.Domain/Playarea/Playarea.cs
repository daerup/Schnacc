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

        public Playarea(PlayareaSize size, IFoodFactory factory)
        {
            this.Size = this.GetValidFieldSize(size);
            this.factory = factory;
            var startPosition = new Position(this.Size.NumberOfRows / 2, this.Size.NumberOfColumns / 2);
            this.Snake = new Snake.Snake(startPosition);
            this.Food = this.GetRandomFoodInUniquePosition();
            this.SetGameState();
        }

        public IFood Food { get; private set; }

        public ISnake Snake { get; }

        public Game CurrentGameState { get; private set; }

        private bool SnakeCollidedWithFood => this.Food.Position.Equals(this.Snake.Head.Position);

        public void MoveSnakeWhenAllowed()
        {
            this.SetGameState();
            if (this.CurrentGameState.Equals(Game.Over))
            {
                return;
            }

            this.Snake.Move();

            if (!this.SnakeCollidedWithFood)
            {
                return;
            }

            this.Snake.Grow();
            this.Food = this.GetRandomFoodInUniquePosition();
        }

        public void UpdateSnakeDirection(Direction newDirection)
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                return;
            }

            this.Snake.UpdateFacingDirection(newDirection);
            this.SetGameState();
        }

        public void RestartGame()
        {
            if (this.CurrentGameState.Equals(Game.Over))
            {
                this.Snake.ResetSnakeToStartPosition();
            }
        }

        private void SetGameState()
        {
            if (this.Snake.CurrentDirection.Equals(Direction.None))
            {
                this.CurrentGameState = Game.Start;
            }
            else if (this.NextPositionIsValid())
            {
                this.CurrentGameState = Game.Running;
            }
            else
            {
                this.CurrentGameState = Game.Over;
            }
        }

        private bool NextPositionIsValid() =>
            !this.NextPositionCollidesWithSnakeBody() && !this.NextPositionCollidesWithWalls();

        private bool NextPositionCollidesWithSnakeBody()
        {
            Position nextHeadPosition = this.Snake.GetNextHeadPosition();
            return this.Snake.Body.Select(sb => sb.Position).Any(p => p.Equals(nextHeadPosition));
        }

        private bool NextPositionCollidesWithWalls()
        {
            Position nextPosition = this.Snake.GetNextHeadPosition();
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
            List<Position> allUsedPositions = this.Snake.Body.Select(bp => bp.Position).ToList();
            allUsedPositions.Add(this.Snake.Head.Position);
            IFood randomFood;

            do
            {
                randomFood = this.factory.CreateRandomFoodBetweenBoundaries(this.GetCorner());
            } while (allUsedPositions.Exists(up => up.Equals(randomFood.Position)));

            return randomFood;
        }
    }
}