namespace Schnacc.Domain.Snake
{
    public class SnakeSegment
    {
        public SnakeSegment(Position position) => this.Position = position;

        public Position Position { get; set; }
    }
}