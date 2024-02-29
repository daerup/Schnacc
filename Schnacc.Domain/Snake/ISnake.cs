using System.Collections.Generic;

namespace Schnacc.Domain.Snake
{
    public interface ISnake
    {
        SnakeHead Head { get; }

        List<SnakeBodyPart> Body { get; }

        Direction CurrentDirection { get; }
    }
}