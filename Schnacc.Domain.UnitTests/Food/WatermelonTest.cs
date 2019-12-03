namespace Schnacc.Domain.UnitTests.Food
{
    using FakeItEasy;

    using FluentAssertions;

    using Schnacc.Domain.Food;

    using Xunit;

    public class WatermelonTest
    {
        private readonly IFood testee = new Watermelon(A.Dummy<Position>());

        [Fact]
        private void watermelonInstanceShouldBeNamedWatermelon()
        {
            // Assert
            this.testee.Name.Should().Be("Watermelon");
        }
    }
}