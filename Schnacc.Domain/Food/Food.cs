namespace Schnacc.Domain.Food
{
    public abstract class Food : IFood
    {
        public string Name => this.GetType().Name;
    }
}
