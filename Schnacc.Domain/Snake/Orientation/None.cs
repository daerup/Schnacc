namespace Schnacc.Domain.Snake.Orientation {
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class None : OrientationState
    {
        public None(OrientationState state) : this(state.Snake)
        {
        }

        public None(Snake snake)
        {
            this.ValidNewDirections = new List<OrientationDirection>
                                          {
                                              OrientationDirection.Forwards,
                                              OrientationDirection.Backwards,
                                              OrientationDirection.Upwards,
                                              OrientationDirection.Downwards
                                          };
            this.Snake = snake;
            this.Snake.orientation = new NoMovement();
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