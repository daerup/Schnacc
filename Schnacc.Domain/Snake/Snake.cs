namespace Schnacc.Domain.Snake
{
    using System.Collections.Generic;
    using System.Linq;
    using Orientation;

    public class Snake
    {
        public Snake(int startRow, int startColumn)
        {
            this.instantiateSnake(startRow, startColumn);
        }

        public IDirectionState FacingDirection { get; set; }

        public Direction CurrentDirection => DirectionState.DirectionToTypeMapper.FirstOrDefault(x => x.Value == this.FacingDirection.GetType()).Key;

        public SnakeHead Head { get; private set; }

        public List<SnakeBodyPart> Body { get; private set; }

        public void ResetSnakeToPosition(int startRow, int startColumn)
        {
            this.instantiateSnake(startRow, startColumn);
        }

        public void Move()
        {
            this.FacingDirection.MoveHead();
            //this.FacingDirection.MoveBody();
        }

        public void Grow()
        {
            this.addBodyPart();
        }

        public void UpdateFacingDirection(Direction newDirection)
        {
            this.FacingDirection.TryChangeDirection(newDirection);
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
            this.FacingDirection = new NoDirection(this);
            this.Head = new SnakeHead(new Position(startRow, startColumn));
            this.Body = new List<SnakeBodyPart>();
            this.UpdateFacingDirection(Direction.None);
        }
    }
}