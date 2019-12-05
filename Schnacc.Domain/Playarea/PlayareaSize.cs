namespace Schnacc.Domain.Playarea
{
    public struct PlayareaSize
    {
        public int NumberOfRows { get; private set; }

        public int NumberOfColumns { get; private set; }

        public PlayareaSize(int numberOfColumns, int numberOfRows)
        {
            this.NumberOfColumns = numberOfColumns;
            this.NumberOfRows = numberOfRows;
        }
    }
}