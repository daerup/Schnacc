namespace Schnacc.Domain.Snake
{
    using System.Collections.Generic;

    public interface ISnake
    {
        SnakeHead Head { get; }

        List<SnakeBodyPart> Body { get; }

        Direction CurrentDirection { get; }
    }
}