﻿namespace Schnacc.Domain.Snake.Orientation
{
    public interface IDirectionState
    {
        void TryChangeDirection(Direction newDirection);
    }
}