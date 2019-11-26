namespace Schnacc.Domain.Snake.Orientation
{
    using System;
    using System.Collections.Generic;

    public abstract class OrientationState
    {
        public Snake Snake { get; set; }

        public OrientationDirection OrientationDirection { get; protected set; }

        public abstract List<OrientationDirection> ValidNewDirections { get; }

        protected bool newDirectionIsValid(OrientationDirection newDirection)
        {
            return this.ValidNewDirections.Contains(newDirection);
        }

        protected OrientationState getNewOrientationState(OrientationDirection newDirection, OrientationState caller)
        {
            switch (newDirection)
            {
                case OrientationDirection.Still:
                    return new None(caller);
                case OrientationDirection.Forwards:
                    return new Forward(caller);
                case OrientationDirection.Backwards:
                    return new Backward(caller);
                case OrientationDirection.Upwards:
                    return new Upward(caller);
                case OrientationDirection.Downwards:
                    return new Downward(caller);
                default:
                    throw new ArgumentOutOfRangeException(nameof(newDirection), newDirection, null);
            }
        }

        public abstract void ChangeDirection(OrientationDirection newDirection);
    }
}