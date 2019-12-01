using FakeItEasy;

namespace Schnacc.Domain.UnitTests.Food
{
    using FluentAssertions;

    using Schnacc.Domain.Food;

    using Xunit;

    public class AppleTest
    {
        private readonly IFood testee = new Apple(A.Dummy<Position>());

        [Fact]
        public void AppleInstanceShouldBeNamedApple()
        {
            // Assert
            this.testee.Name.Should().Be("Apple");
        }
    }
}