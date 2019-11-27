namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class UpwardDirection : DirectionState
    {
        public UpwardDirection(DirectionState state)
        {
            this.ValidNewDirections =
                new List<Direction> { Direction.Right, Direction.Left };
            this.Snake = state.Snake;
            this.Snake.MovementStrategy = new UpwardMovement();
        }

        protected override sealed List<Direction> ValidNewDirections { get; }
    }
}