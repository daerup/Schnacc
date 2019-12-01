namespace Schnacc.Domain.Food
{
    public abstract class Food : IFood
    {
        protected Food(Position position)
        {
            this.Position = position;
        }

        public string Name => this.GetType().Name;
        public Position Position { get; set; }
    }
}
