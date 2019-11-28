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
    }
}
