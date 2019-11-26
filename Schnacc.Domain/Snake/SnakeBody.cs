namespace Schnacc.Domain.Snake
{
    public class SnakeBody
    {
        public (int row, int column) Position;

        public SnakeBody((int row, int column) position)
        {
            this.Position = position;
        }
    }
}