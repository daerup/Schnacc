namespace Schnacc.Domain.Test.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;

    using Xunit;

    public class WatermelonTest
    {
        private readonly Watermelon testee = new Watermelon();

        [Fact]
        private void watermelonInstanceShouldBeNamedWatermelon()
        {
            // Assert
            this.testee.Name.Should().Be("Watermelon");
        }
    }
}