namespace Schnacc.Domain.Snake
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Schnacc.Domain.Snake.Movement;
    using Schnacc.Domain.Snake.Orientation;

    public class Snake
    {
        public Snake(int startRow, int startColumn)
        {
            this.instantiateSnake(startRow, startColumn);
        }

        public IMovement MovementStrategy { private get; set; }

        public Direction CurrentDirection => Orientation.DirectionState.dictionary.FirstOrDefault(x => x.Value == this.DirectionState.GetType()).Key;

        public IDirectionState DirectionState { get; set; }

        public SnakeHead Head { get; private set; }

        public List<SnakeBodyPart> Body { get; private set; }

        public void ResetSnakeToPosition(int startRow, int startColumn)
        {
            this.instantiateSnake(startRow, startColumn);
        }

        public void Move()
        {
            (int, int) newHeadPosition = this.MovementStrategy.Move(this.Head.Position.row, this.Head.Position.column);
            this.Head.Position = newHeadPosition;
        }

        public void Grow()
        {
            this.addBodyPart();
        }

        public void UpdateFacingDirection(Direction newDirection)
        {
            this.DirectionState.ChangeDirection(newDirection);
        }

        private void addBodyPart()
        {
            if (this.Body.Any())
            {
                this.Body.Add(new SnakeBodyPart(this.Body.Last().Position));
                return;
            }

            this.Body.Add(new SnakeBodyPart(this.Head.Position));
        }

        private void instantiateSnake(int startRow, int startColumn)
        {
            this.DirectionState = new NoDirection(this);
            this.Head = new SnakeHead(startRow, startColumn);
            this.Body = new List<SnakeBodyPart>();
            this.UpdateFacingDirection(Direction.None);
        }
    }
}