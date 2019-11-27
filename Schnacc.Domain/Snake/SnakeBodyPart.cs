namespace Schnacc.Domain.Snake
{
    public class SnakeBodyPart
    {
        public (int row, int column) Position;

        public SnakeBodyPart((int row, int column) position)
        {
            this.Position = position;
        }
    }
}