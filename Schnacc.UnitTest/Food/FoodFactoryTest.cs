using FakeItEasy;

namespace Schnacc.Domain.UnitTests.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;

    using Xunit;

    public class FoodFactoryTest
    {
        private FoodFactory testee;

        [Theory]
        [Repeat(30)]
        private void createRandomFoodShouldCreateFoodType()
        {
            // Act
            this.testee = new FoodFactory(A.Dummy<(int, int)>());
            IFood createdFood = this.testee.CreateRandomFood();

            // Assert
            createdFood.Should().NotBeNull();
            createdFood.Should().Match(f => f is Apple || f is Banana || f is Watermelon);
        }

        [Theory]
        [Repeat(30)]
        private void createRandomFoodShouldCreateFoodAtRandomPosition()
        {
            // Act
            this.testee = new FoodFactory((10, 10));
            IFood createdFood = this.testee.CreateRandomFood();

            // Assert
            createdFood.Should().NotBeNull();
            createdFood.Position.Row.Should().BeInRange(1, 9);
            createdFood.Position.Column.Should().BeInRange(1, 9);
        }
    }
}