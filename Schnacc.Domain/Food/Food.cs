namespace Schnacc.Domain.Food
{
    public interface IFood
    {
        string Name { get; }
    }

    public abstract class Food : IFood
    {
        public string Name => this.GetType().Name;
    }
}
