using FakeItEasy;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Food
{
    public class WatermelonTest
    {
        private readonly IFood _testee = new Watermelon(A.Dummy<Position>());

        [Fact]
        public void WatermelonInstanceShouldBeNamedWatermelon()
        {
            // Assert
            this._testee.Name.Should().Be("Watermelon");
        }
    }
}