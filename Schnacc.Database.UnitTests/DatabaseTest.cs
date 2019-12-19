namespace Schnacc.Database.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Schnacc.Authorization;

    using Xunit;

    public class DatabaseTest
    {
        private Database testee;

        [Fact]
        private void test()
        {
            this.testee = new Database(new AuthorizationApi().SignInAnonymously());
            List<Highscore> highscores = this.testee.GetHighscores();

            highscores.Count.Should().Be(2);
        }
    }
}
