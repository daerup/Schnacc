using FakeItEasy;

namespace Schnacc.Domain.UnitTests.Food
{
    using FluentAssertions;
    using Schnacc.Domain.Food;
    using Xunit;

    public class BananaTest
    {
        private readonly Banana testee = new Banana(A.Dummy<Position>());

        [Fact]
        private void bananaInstanceShouldBeNamedBanana()
        {
            // Assert
            this.testee.Name.Should().Be("Banana");
        }
    }
}