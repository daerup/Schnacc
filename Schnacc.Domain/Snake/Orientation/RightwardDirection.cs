namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    public class RightwardDirection : DirectionState
    {
        public RightwardDirection(DirectionState state)
        {
            this.ValidNewDirections =
                new List<Direction> { Direction.Up, Direction.Down };
            this.Snake = state.Snake;
        }

        protected override List<Direction> ValidNewDirections { get; }

        public override void MoveHead()
        {
            this.Snake.Head.Position = this.GetNexPosition();
        }

        public override Position GetNexPosition()
        {
            Position previousPosition = this.Snake.Head.Position;
            return new Position(previousPosition.Row, previousPosition.Column + 1);
        }
    }
}