namespace Schnacc.Domain.Snake
{
    public class SnakeHead
    {
        public (int row, int column) Position;

        public SnakeHead(int row, int column)
        {
            this.Position.row = row;
            this.Position.column = column;
        }
    }
}