namespace Schnacc.Domain.Snake.Orientation {
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class Upward : OrientationState
    {
        private Upward(OrientationState state)
        {
            this.ValidNewDirections =
                new List<OrientationDirection> { OrientationDirection.Forwards, OrientationDirection.Backwards };
            this.Snake = state.Snake;
            this.Snake.orientation = new UpwardMovement();
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