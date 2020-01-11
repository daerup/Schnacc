namespace Schnacc.Domain.Snake
{
    using System.Collections.Generic;
    using System.Linq;
    using Orientation;

    public class Snake : ISnake
    {
        public Snake(Position starPosition)
        {
            this.instantiateSnake(starPosition);
        }

        public IDirectionState FacingDirection { private get; set; }

        public Direction CurrentDirection => this.FacingDirection.GetDirectionFromDirectionState();

        public bool HasCollidedWithItSelf => this.Body.Skip(3).Any(bp => bp.Position.Equals(this.Head.Position));

        public Position GetNextHeadPosition() => this.FacingDirection.GetNexPosition();

        public SnakeHead Head { get; private set; }

        public List<SnakeBodyPart> Body { get; private set; }

        public void ResetSnakeToPosition(Position startPosition)
        {
            this.instantiateSnake(startPosition);
        }

        public void Move()
        {
            this.FacingDirection.MoveBody();
            this.FacingDirection.MoveHead();
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

        private void instantiateSnake(Position startPosition)
        {
            this.FacingDirection = new NoDirection(this);
            this.Head = new SnakeHead(startPosition);
            this.Body = new List<SnakeBodyPart>();
            this.UpdateFacingDirection(Direction.None);
        }
    }
}