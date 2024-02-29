namespace Schnacc.Domain.Snake
{
    public class SnakeHead
    {
        public SnakeHead(Position position) => this.Position = position;

        public Position Position { get; set; }
    }
}