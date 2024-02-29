using System.Collections.Generic;
using System.Linq;
using Schnacc.Domain.Snake.Orientation;

namespace Schnacc.Domain.Snake
{
    public class Snake : ISnake
    {
        private Position startPosition;
        public Snake(Position startPosition)
        {
            this.startPosition = startPosition;
            this.InstantiateSnake();
        }

        public IDirectionState FacingDirection { private get; set; }

        public Direction CurrentDirection => this.FacingDirection.GetDirectionFromDirectionState();

        public bool HasCollidedWithItSelf => this.Body.Skip(3).Any(bp => bp.Position.Equals(this.Head.Position));

        public Position GetNextHeadPosition() => this.FacingDirection.GetNexPosition();

        public SnakeHead Head { get; private set; }

        public List<SnakeBodyPart> Body { get; private set; }

        public void ResetSnakeToStartPosition()
        {
            this.InstantiateSnake();
        }

        public void Move()
        {
            this.FacingDirection.MoveBody();
            this.FacingDirection.MoveHead();
        }

        public void Grow()
        {
            this.AddBodyPart();
        }

        public void UpdateFacingDirection(Direction newDirection)
        {
            this.FacingDirection.TryChangeDirection(newDirection);
        }

        private void AddBodyPart()
        {
            if (this.Body.Any())
            {
                this.Body.Add(new SnakeBodyPart(this.Body.Last().Position));
                return;
            }

            this.Body.Add(new SnakeBodyPart(this.Head.Position));
        }

        private void InstantiateSnake()
        {
            this.FacingDirection = new NoDirection(this);
            this.Head = new SnakeHead(this.startPosition);
            this.Body = new List<SnakeBodyPart>();
            this.UpdateFacingDirection(Direction.None);
        }
    }
}