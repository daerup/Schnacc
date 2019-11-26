namespace Schnacc.Domain.Snake.Movement
{
    public class UpwardMovement : IMovement
    {
        public (int, int) Move(int row, int column)
        {
            return (row - 1, column);
        }
    }
}