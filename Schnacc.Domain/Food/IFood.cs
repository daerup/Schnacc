using Schnacc.Domain.Playarea;

namespace Schnacc.Domain.Food
{
    public interface IFood
    {
        string Name { get; }
        Position Position { get; }
    }
}