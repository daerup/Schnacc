namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    using Schnacc.Domain.Snake.Movement;

    public class NoDirection : DirectionState
    {
        public NoDirection(Snake snake)
        {
            this.ValidNewDirections = new List<Direction>
                                          {
                                              Direction.Right,
                                              Direction.Left,
                                              Direction.Up,
                                              Direction.Down
                                          };
            this.Snake = snake;
            this.Snake.MovementStrategy = new NoMovement();
        }

        protected override sealed List<Direction> ValidNewDirections { get; }
    }
}