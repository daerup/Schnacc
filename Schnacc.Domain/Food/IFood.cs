namespace Schnacc.Domain.Food
{
    using Schnacc.Domain.Snake;

    public interface IFood
    {
        string Name { get; }

        Position Position { get; }
    }
}