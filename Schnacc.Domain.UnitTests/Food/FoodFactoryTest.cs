namespace Schnacc.Domain.UnitTests.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;
    using Schnacc.Domain.Snake;

    using Xunit;

    public class FoodFactoryTest
    {
        private FoodFactory testee;

        [Fact]
        private void createRandomFoodShouldCreateFoodType()
        {
            for (int i = 0; i < 30; i++)
            {
                // Act
                this.testee = new FoodFactory();
                IFood createdFood = this.testee.CreateRandomFoodBetweenBoundaries(new Position(10, 10));

                // Assert
                createdFood.Should().NotBeNull();
                createdFood.Should().Match(f => f is Apple || f is Banana || f is Watermelon); 
            }
        }

        [Fact]
        private void createRandomFoodShouldCreateFoodAtRandomPosition()
        {
            for (int i = 0; i < 30; i++)
            {
                // Act
                this.testee = new FoodFactory();
                IFood createdFood = this.testee.CreateRandomFoodBetweenBoundaries(new Position(10, 10));

                // Assert
                createdFood.Should().NotBeNull();
                createdFood.Position.Row.Should().BeInRange(1, 9);
                createdFood.Position.Column.Should().BeInRange(1, 9); 
            }
        }
    }
}