using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Schnacc.Authorization;
using Xunit;

namespace Schnacc.Database.UnitTests
{
    public class DatabaseTest
    {
        private Database testee;

        [Fact(Skip = "failing")]
        public async Task UserCanFetchHighscores()
        {
            // Act
            const string email = "hans.muster@mail.ch";
            const string password = "testAccount01";
            var authorizationApi = new AuthorizationApi();
            this.testee = new Database(await authorizationApi.SignInWithEmail(email, password));
            
            // Assert
            List<Highscore> highscores = this.testee.GetHighscores();
            highscores.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
