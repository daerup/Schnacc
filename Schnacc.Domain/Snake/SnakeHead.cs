namespace Schnacc.Domain.Snake
{
    public class SnakeHead
    {
        public SnakeHead(int row, int column)
        {
            this.Position = (row, column);
        }

        public (int row, int column) Position { get; set; }
    }
}