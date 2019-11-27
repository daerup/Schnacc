﻿namespace Schnacc.Domain.Test.Food
{
    using FluentAssertions;
    using Schnacc.Domain.Food;
    using Xunit;

    public class BananaTest
    {
        private readonly IFood testee = new Banana();

        [Fact]
        private void bananaInstanceShouldBeNamedBanana()
        {
            // Assert
            this.testee.Name.Should().Be("Banana");
        }
    }
}