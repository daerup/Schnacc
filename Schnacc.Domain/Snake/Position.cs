using System.Collections;

namespace Schnacc.Domain.Snake
{
    public class Position : IEqualityComparer
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

        public new bool Equals(object x, object y) => x is Position positionX && y is Position positionY && positionX.Row == positionY.Row && positionX.Column == positionY.Column;

        public int GetHashCode(object obj)
        {
            if (obj is Position position)
            {
                return position.Row.GetHashCode() ^ position.Column.GetHashCode();
            }

            return 0;
        }
    }
}
