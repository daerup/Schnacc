namespace Schnacc.Domain.UnitTests.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;

    using Xunit;

    public class FoodFactoryTest
    {
        private readonly FoodFactory testee = new FoodFactory();

        // TODO Implement random position for Food
        [Theory]
        [repeat(30)]
        private void createRandomFoodShouldCreateFood()
        {
            // Act
            IFood createdFood = this.testee.CreateRandomFood();

            // Assert
            createdFood.Should().NotBeNull();
            createdFood.Should().Match(f => f is Apple || f is Banana || f is Watermelon);
        }
    }
}