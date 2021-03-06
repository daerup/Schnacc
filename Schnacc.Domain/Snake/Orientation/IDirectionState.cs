﻿namespace Schnacc.Domain.Snake.Orientation
{
    public interface IDirectionState
    {
        void TryChangeDirection(Direction newDirection);

        void MoveHead();

        void MoveBody();

        Position GetNexPosition();

        Direction GetDirectionFromDirectionState();
    }
}