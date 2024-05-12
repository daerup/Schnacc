using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Schnacc.Authorization;
using System.Linq;
using Xunit;

namespace Schnacc.Database.UnitTests
{
    public class FirebaseDatabaseTest
    {
        private FirebaseDatabase _testee;

        [Fact]
        private async Task UserCanFetchHighscores()
        {
            // Act
            var authConfig = new AuthConfig { ApiKey = "AIzaSyDt-cGm2puZ2fFKspUCenrRe-Mfie-gRWM" };
            var authorizationApi = new AuthorizationApi(authConfig);

            string key = await authorizationApi.SignInAnonymous();
            var dbConfig = new FirebaseDatabaseConfig { SessionKey = key };
            this._testee = new FirebaseDatabase(dbConfig);
            
            // Assert
            var highscores = this._testee.GetHighscores().ToList();
            highscores.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
