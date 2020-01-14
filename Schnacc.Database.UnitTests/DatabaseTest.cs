using System.Threading.Tasks;

namespace Schnacc.Database.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Schnacc.Authorization;

    using Xunit;

    public class DatabaseTest
    {
        private Database testee;

        [Fact]
        private async void UserCanFetchHighscores()
        {
            // Act
            string email = "hans.muster@mail.ch";
            string password = "testAccount01";
            AuthorizationApi authorizationApi = new AuthorizationApi();
            Database database = this.testee = new Database(await authorizationApi.SignInWithEmail(email, password));
            
            // Act
            List<Highscore> highscores = this.testee.GetHighscores();

            highscores.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
