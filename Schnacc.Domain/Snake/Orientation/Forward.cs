namespace Schnacc.Domain.Snake.Orientation
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;

    using Schnacc.Domain.Food;
    using Schnacc.Domain.Snake.Movement;

    public class Forward : OrientationState
    {
        public Forward(OrientationState state)
        {
            this.ValidNewDirections =
                new List<OrientationDirection> { OrientationDirection.Upwards, OrientationDirection.Downwards };
            this.Snake = state.Snake;
            this.Snake.orientation = new ForwardMovement();
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