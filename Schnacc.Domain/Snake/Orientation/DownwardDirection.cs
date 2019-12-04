namespace Schnacc.Domain.Snake.Orientation
{
    using System.Collections.Generic;

    public class DownwardDirection : DirectionState
    {
        public DownwardDirection(DirectionState state)
        {
            this.ValidNewDirections =
                new List<Direction> { Direction.Right, Direction.Left };
            this.Snake = state.Snake;
        }

        protected override List<Direction> ValidNewDirections { get; }

        public override void MoveHead()
        {
            Position previousPosition = this.Snake.Head.Position;
            this.Snake.Head.Position = new Position(previousPosition.Row + 1, previousPosition.Column);
        }
    }
}