using FakeItEasy;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Xunit;

namespace Schnacc.Domain.UnitTests.Food
{
    public class BananaTest
    {
        private readonly IFood _testee = new Banana(A.Dummy<Position>());

        [Fact]
        public void BananaInstanceShouldBeNamedBanana()
        {
            // Assert
            this._testee.Name.Should().Be("Banana");
        }
    }
}