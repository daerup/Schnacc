namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

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
        }

        protected override List<Direction> ValidNewDirections { get; }

        public override void MoveHead()
        {
            Position previousPosition = this.Snake.Head.Position;
            this.Snake.Head.Position = new Position(previousPosition.Row, previousPosition.Column);
        }
    }
}