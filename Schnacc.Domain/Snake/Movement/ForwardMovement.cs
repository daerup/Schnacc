﻿namespace Schnacc.Domain.Snake.Movement
{
    public class ForwardMovement : IMovement
    {
        public (int, int) Move(int row, int column)
        {
            return (row, column + 1);
        }
    }
}