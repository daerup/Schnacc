namespace Schnacc.Domain.Test.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;
    using Schnacc.Domain.Test;

    using Xunit;

    public class FoodFactoryTest
    {
        private readonly FoodFactory testee = new FoodFactory();

        [Theory]
        [Repeat(100)]
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