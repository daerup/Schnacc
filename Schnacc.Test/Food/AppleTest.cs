namespace Schnacc.Domain.Test.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;

    using Xunit;

    public class AppleTest
    {
        private readonly Apple testee = new Apple();

        [Fact]
        private void appleInstanceShouldBeNamedApple()
        {
            // Assert
            this.testee.Name.Should().Be("Apple");
        }
    }
}