namespace Schnacc.Domain.Snake
{
    public class SnakeBodyPart
    {
        public SnakeBodyPart(Position position)
        {
            this.Position = position;
        }

        public Position Position { get; }
    }
}