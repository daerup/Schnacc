namespace Schnacc.Domain.Playarea
{
    public struct PlayareaSize
    {
        public int NumberOfRows { get; private set; }

        public int NumberOfColumns { get; private set; }

        public PlayareaSize(int numberOfRows, int numberOfColumns)
        {
            this.NumberOfRows = numberOfRows;
            this.NumberOfColumns = numberOfColumns;
        }
    }
}