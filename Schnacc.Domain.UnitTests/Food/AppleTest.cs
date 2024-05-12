using FakeItEasy;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Playarea;
using Xunit;

namespace Schnacc.Domain.UnitTests.Food
{
    public class AppleTest
    {
        private readonly IFood _testee = new Apple(A.Dummy<Position>());

        [Fact]
        public void AppleInstanceShouldBeNamedApple()
        {
            // Assert
            this._testee.Name.Should().Be("Apple");
        }
    }
}