namespace Schnacc.Domain.Snake.Movement
{
    public class DownwardMovement : IMovement
    {
        public (int, int) Move(int row, int column)
        {
            return (row + 1, column);
        }
    }
}