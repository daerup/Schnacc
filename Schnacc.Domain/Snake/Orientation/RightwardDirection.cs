namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class RightwardDirection : DirectionState
    {
        public RightwardDirection(DirectionState state)
        {
            this.ValidNewDirections =
                new List<Direction> { Direction.Up, Direction.Down };
            this.Snake = state.Snake;
            this.Snake.MovementStrategy = new RightwardMovement();
        }

        protected override sealed List<Direction> ValidNewDirections { get; }
    }
}