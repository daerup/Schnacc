namespace Schnacc.Domain.Snake.Orientation
{
    public interface IDirectionState
    {
        void ChangeDirection(Direction newDirection);
    }
}