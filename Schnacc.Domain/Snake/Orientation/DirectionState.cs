using Schnacc.Domain.Playarea;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schnacc.Domain.Snake.Orientation
{
    public abstract class DirectionState : IDirectionState
    {
        private readonly Dictionary<Direction, Type> _directionToTypeMapper = new Dictionary<Direction, Type>
                                                                      {
                                                                          { Direction.Right, typeof(RightwardDirection) },
                                                                          { Direction.Left, typeof(LeftwardDirection) },
                                                                          { Direction.Up, typeof(UpwardDirection) },
                                                                          { Direction.Down, typeof(DownwardDirection) },
                                                                          { Direction.None, typeof(NoDirection) }
                                                                      };

        public Snake Snake { get; protected set; }

        protected abstract List<Direction> ValidNewDirections { get; }

        public Direction GetDirectionFromDirectionState()
        {
            return this._directionToTypeMapper.FirstOrDefault(x => x.Value == this.GetType()).Key;
        }

        // ReSharper disable once FlagArgument
        public void TryChangeDirection(Direction newDirection)
        {
            if (this.NewDirectionIsValid(newDirection))
            {
                this.Snake.FacingDirection = this.GetNewOrientationState(newDirection, this);
            }
        }

        public abstract void MoveHead();

        public abstract Position GetNexPosition();

        public void MoveBody()
        {
            if (this.Snake.Body.Count == 0)
            {
                return;
            }

            this.MoveBodyParts();
        }

        private void MoveBodyParts()
        {
            for (int i = this.Snake.Body.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    this.Snake.Body[i].Position = new Position(this.Snake.Head.Position);
                    continue;
                }

                this.Snake.Body[i].Position = new Position(this.Snake.Body[i - 1].Position);
            }
        }

        private bool NewDirectionIsValid(Direction newDirection) => this.ValidNewDirections.Contains(newDirection);

        private DirectionState GetNewOrientationState(Direction newDirection, DirectionState caller) => (DirectionState)Activator.CreateInstance(this._directionToTypeMapper[newDirection], caller);
    }
}