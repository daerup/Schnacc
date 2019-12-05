namespace Schnacc.Domain.UnitTests.Food
{
    using FakeItEasy;

    using FluentAssertions;

    using Schnacc.Domain.Food;
    using Schnacc.Domain.Snake;

    using Xunit;

    public class AppleTest
    {
        private readonly IFood testee = new Apple(A.Dummy<Position>());

        [Fact]
        private void appleInstanceShouldBeNamedApple()
        {
            // Assert
            this.testee.Name.Should().Be("Apple");
        }
    }
}