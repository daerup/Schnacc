namespace Schnacc.Domain.Snake
{
    public class SnakeBodyPart
    {
        public SnakeBodyPart((int row, int column) position)
        {
            this.Position = position;
        }

        public (int row, int column) Position { get; }
    }
}