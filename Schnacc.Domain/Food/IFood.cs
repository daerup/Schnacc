﻿using Schnacc.Domain.Snake;

namespace Schnacc.Domain.Food
{
    public interface IFood
    {
        string Name { get; }

        Position Position { get; }
    }
}