using System.Collections.Generic;

namespace Schnacc.Domain.Snake
{
    public interface ISnake
    {
        SnakeSegment Head { get; }
        List<SnakeSegment> Body { get; }
        Direction CurrentDirection { get; }
        void Move();
        void Grow();
        void UpdateFacingDirection(Direction newDirection);
        void ResetSnakeToStartPosition();
        Position GetNextHeadPosition();
    }
}