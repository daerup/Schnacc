namespace Schnacc.Domain.Food
{
    public interface IFoodFactory
    {
        Food CreateRandomFoodBetweenBoundaries(Position boundaries);
    }
}