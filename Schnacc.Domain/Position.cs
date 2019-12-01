namespace Schnacc.Domain
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public Position(Position position)
        {
            this.Row = position.Row;
            this.Column = position.Column;
        }

        public bool Equals(Position other)
        {
            return this.Row == other.Row && this.Column == other.Column;
        }
    }
}
