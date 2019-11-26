namespace Schnacc.Domain.Snake.Orientation {
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class Backward : OrientationState
    {
        public Backward(OrientationState state) : this(state.Snake)
        {
        }

        public Backward(Snake snake)
        {
            this.ValidNewDirections = new List<OrientationDirection>();
            this.ValidNewDirections.Add(OrientationDirection.Upwards);
            this.ValidNewDirections.Add(OrientationDirection.Downwards);
            this.Snake = snake;
            this.Snake.orientation = new BackwardMovement();
        }

        public override sealed List<OrientationDirection> ValidNewDirections { get; }

        public override void ChangeDirection(OrientationDirection newDirection)
        {
            if (this.newDirectionIsValid(newDirection))
            {
                this.Snake.Facing = this.getNewOrientationState(newDirection, this);
            }
        }
    }
}