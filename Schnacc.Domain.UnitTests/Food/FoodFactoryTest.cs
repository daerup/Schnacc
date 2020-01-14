namespace Schnacc.Domain.UnitTests.Food
{
    using System;

    using FluentAssertions;

    using FsCheck;
    using FsCheck.Xunit;

    using Schnacc.Domain.Food;
    using Schnacc.Domain.Snake;

    using Xunit;

    public class FoodFactoryTest
    {
        private FoodFactory testee;

        [Fact]
        private void CreateRandomFoodShouldCreateFoodType()
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
        private void CreateRandomFoodShouldCreateFoodAtRandomPosition()
        {
            for (int i = 0; i < 30; i++)
            {
                // Act
                this.testee = new FoodFactory();
                IFood createdFood = this.testee.CreateRandomFoodBetweenBoundaries(new Position(10, 10));

                // Assert
                createdFood.Should().NotBeNull();
                createdFood.Position.Row.Should().BeInRange(0, 9);
                createdFood.Position.Column.Should().BeInRange(0, 9); 
            }
        }

        [Property]
        public Property CreatedFoodShouldBeBetweenBounderies(int row, int column )
        {
            Func<bool> func = () =>
                {
                    this.testee = new FoodFactory();
                    IFood createdFood = this.testee.CreateRandomFoodBetweenBoundaries(new Position(row, column));

                    return (0 <= createdFood.Position.Row && createdFood.Position.Row <= row && 0 <= createdFood.Position.Column && createdFood.Position.Column <= column);
                };

            return func.When(row > 0 && column > 0);
        }
    }
}