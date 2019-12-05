namespace Schnacc.Domain.Food
{
    using Schnacc.Domain.Snake;

    public abstract class Food : IFood
    {
        protected Food(Position position)
        {
            this.Position = position;
        }

        public string Name => this.GetType().Name;

        public Position Position { get; }
    }
}
