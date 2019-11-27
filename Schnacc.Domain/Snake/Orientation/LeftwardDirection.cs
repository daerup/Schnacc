namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class LeftwardDirection : DirectionState
    {
        public LeftwardDirection(DirectionState state)
        {
            this.ValidNewDirections =
                new List<Direction> { Direction.Up, Direction.Down };
            this.Snake = state.Snake;
            this.Snake.MovementStrategy = new LeftwardsMovement();
        }

        protected override sealed List<Direction> ValidNewDirections { get; }
    }
}