using FakeItEasy;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Food
{
    public class BananaTest
    {
        private readonly IFood testee = new Banana(A.Dummy<Position>());

        [Fact]
        public void BananaInstanceShouldBeNamedBanana()
        {
            // Assert
            this.testee.Name.Should().Be("Banana");
        }
    }
}