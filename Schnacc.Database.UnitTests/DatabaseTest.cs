using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Schnacc.Authorization;
using System.Linq;
using Xunit;

namespace Schnacc.Database.UnitTests
{
    public class DatabaseTest
    {
        private Database _testee;

        [Fact]
        private async Task UserCanFetchHighscores()
        {
            // Act
            const string email = "hans.muster@mail.ch";
            const string password = "testAccount01";
            var authorizationApi = new AuthorizationApi();
            this._testee = new Database(await authorizationApi.SignInWithEmail(email, password));
            
            // Assert
            var highscores = this._testee.GetHighscores().ToList();
            highscores.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
