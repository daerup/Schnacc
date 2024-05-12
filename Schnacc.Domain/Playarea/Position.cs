namespace Schnacc.Domain.Playarea
{
    public sealed class Position
    {
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

        public int Row { get; }

        public int Column { get; }

        public bool Equals(Position other) => this.Row == other.Row && this.Column == other.Column;
    }
}
