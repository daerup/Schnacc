namespace Schnacc.Domain.Food
{
    using Schnacc.Domain.Snake;

    public interface IFoodFactory
    {
        Food CreateRandomFoodBetweenBoundaries(Position boundaries);
    }
}