using FakeItEasy;
using FluentAssertions;
using Schnacc.Domain.Food;
using Schnacc.Domain.Snake;
using Xunit;

namespace Schnacc.Domain.UnitTests.Food
{
    public class AppleTest
    {
        private readonly IFood _testee = new Apple(A.Dummy<Position>());

        [Fact]
        private void AppleInstanceShouldBeNamedApple()
        {
            // Assert
            this._testee.Name.Should().Be("Apple");
        }
    }
}