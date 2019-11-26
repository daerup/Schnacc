namespace Schnacc.Domain.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Schnacc.Domain.Snake.Movement;
    using Schnacc.Domain.Snake.Orientation;

    public class Snake
    {
        public SnakeHead Head;

        public List<SnakeBody> Body;

        public IMovement orientation;

        public OrientationState Facing { get; set; }

        public Snake(int startRow, int startColumn)
        {
            this.instantiateSnake(startRow, startColumn);
        }

        public void ResetSnakeToPosition(int startRow, int startColumn)
        {
            this.instantiateSnake(startRow, startColumn);
        }

        public void Move()
        {
            (int, int) newHeadPosition = this.orientation.Move(this.Head.Position.row, this.Head.Position.column);
            this.Head.Position = newHeadPosition;
        }

        public void Eat()
        {
            (int row, int column) lastPosition = this.Body.Last().Position;
            this.Body.Add(new SnakeBody(lastPosition));
        }

        public void UpdateOrientation(OrientationDirection orientationStateOfHead)
        {
            this.Facing.ChangeDirection(orientationStateOfHead);
        }

        private void instantiateSnake(int startRow, int startColumn)
        {
            this.Facing = new None(this);
            this.Head = new SnakeHead(startRow, startColumn);
            this.Body = new List<SnakeBody>();
            this.UpdateOrientation(OrientationDirection.Still);
        }
    }
}