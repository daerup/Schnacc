namespace Schnacc.Domain.Snake.Movement
{
    public class NoMovement : IMovement
    {
        public (int, int) Move(int row, int column)
        {
            return (row, column);
        }
    }
}