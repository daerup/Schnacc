namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    public class LeftwardDirection : DirectionState
    {
        public LeftwardDirection(DirectionState state)
        {
            this.ValidNewDirections =
                new List<Direction> { Direction.Up, Direction.Down };
            this.Snake = state.Snake;
        }

        protected override List<Direction> ValidNewDirections { get; }

        public override void MoveHead()
        {
            Position previousPosition = this.Snake.Head.Position;
            this.Snake.Head.Position = new Position(previousPosition.Row, previousPosition.Column - 1);
        }
    }
}