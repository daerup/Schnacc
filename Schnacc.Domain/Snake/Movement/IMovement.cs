namespace Schnacc.Domain.Snake.Movement
{
    public interface IMovement
    {
        (int, int) Move(int row, int column);
    }
}