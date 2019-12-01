using System.Linq;

namespace Schnacc.Domain.Snake.Orientation
{
    using System;
    using System.Collections.Generic;

    public abstract class DirectionState : IDirectionState
    {
        public static readonly Dictionary<Direction, Type> DirectionToTypeMapper = new Dictionary<Direction, Type>
                                                                      {
                                                                          { Direction.Right, typeof(RightwardDirection) },
                                                                          { Direction.Left, typeof(LeftwardDirection) },
                                                                          { Direction.Up, typeof(UpwardDirection) },
                                                                          { Direction.Down, typeof(DownwardDirection) },
                                                                          { Direction.None, typeof(NoDirection) }
                                                                      };

        public Snake Snake { get; protected set; }

        protected abstract List<Direction> ValidNewDirections { get; }


        // ReSharper disable once FlagArgument
        public void TryChangeDirection(Direction newDirection)
        {
            if (this.newDirectionIsValid(newDirection))
            {
                this.Snake.FacingDirection = this.getNewOrientationState(newDirection, this);
            }
        }

        public abstract void MoveHead();

        public void MoveBody()
        {
            if (this.Snake.Body.Any() == false)
            {
                return;
            }

            this.moveEveryBodyPart();
        }

        private void moveEveryBodyPart()
        {
            for (int i = this.Snake.Body.Count-1; i >= 0; i--)
            {
                if (i == 0)
                {
                    this.Snake.Body[i].Position = new Position(this.Snake.Head.Position);
                    continue;
                }
                this.Snake.Body[i].Position = new Position(this.Snake.Body[i - 1].Position);
            }
        }

        private bool newDirectionIsValid(Direction newDirection)
        {
            return this.ValidNewDirections.Contains(newDirection);
        }

        private DirectionState getNewOrientationState(Direction newDirection, DirectionState caller)
        {
            return (DirectionState)Activator.CreateInstance(DirectionToTypeMapper[newDirection], caller);
        }
    }
}