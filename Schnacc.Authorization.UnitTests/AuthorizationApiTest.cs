using System;
using System.Threading.Tasks;
using FluentAssertions;
using Schnacc.Authorization.Exception;
using Xunit;

namespace Schnacc.Authorization.UnitTests
{
    public class AuthorizationApiTest
    {
        private readonly AuthorizationApi testee = new AuthorizationApi();

        [Fact(Skip = "failing")]
        public async Task UserCanSignInWithEmailAndPassword()
        {
            // Arrange
            const string email = "hans.muster@mail.ch";
            const string password = "testAccount01";
            
            // Act
            string signInToken = await this.testee.SignInWithEmail(email, password);

            // Assert
            signInToken.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "failing")]
        public void WhenNotRegisteredUserCanNotSignIn()
        {
            // Arrange
            const string email = "hans.muster@notRegistered.ch";
            const string password = "testAccount01";

            // Act
            Func<Task> func = async () => await this.testee.SignInWithEmail(email, password);

            // Assert
            func.Should().Throw<UserNotRegisteredException>();
        }
    }
}