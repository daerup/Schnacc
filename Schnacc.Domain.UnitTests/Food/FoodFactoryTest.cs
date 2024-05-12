using System;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Food
{
    public class FoodFactoryTest
    {
        private FoodFactory _testee;

        [Fact]
        public void CreateRandomFoodShouldCreateFoodType()
        {
            for (int i = 0; i < 30; i++)
            {
                // Act
                this._testee = new FoodFactory();
                IFood createdFood = this._testee.CreateRandomFoodBetweenBoundaries(new Position(10, 10));

                // Assert
                createdFood.Should().NotBeNull();
                createdFood.Should().Match(f => f is Apple || f is Banana || f is Watermelon); 
            }
        }

        [Fact]
        public void CreateRandomFoodShouldCreateFoodAtRandomPosition()
        {
            for (int i = 0; i < 30; i++)
            {
                // Act
                this._testee = new FoodFactory();
                IFood createdFood = this._testee.CreateRandomFoodBetweenBoundaries(new Position(10, 10));

                // Assert
                createdFood.Should().NotBeNull();
                createdFood.Position.Row.Should().BeInRange(0, 9);
                createdFood.Position.Column.Should().BeInRange(0, 9); 
            }
        }
    }
}