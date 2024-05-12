using System.Collections.Generic;

namespace Schnacc.Domain.Snake.Orientation
{
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
            this.Snake.Head.Position = this.GetNexPosition();
        }

        public override Position GetNexPosition()
        {
            var previousPosition = this.Snake.Head.Position;
            return new Position(previousPosition.Row, previousPosition.Column - 1);
        }
    }
}