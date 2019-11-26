namespace Schnacc.Domain.Snake.Movement
{
    public class BackwardMovement : IMovement
    {
        public (int, int) Move(int row, int column)
        {
            return (row, column - 1);
        }
    }
}