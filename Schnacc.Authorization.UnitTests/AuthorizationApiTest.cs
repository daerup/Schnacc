using System.Threading.Tasks;

namespace Schnacc.Authorization.UnitTests
{
    using System;

    using FluentAssertions;

    using Schnacc.Authorization.Exception;

    using Xunit;

    public class AuthorizationApiTest
    {
        private AuthorizationApi testee = new AuthorizationApi();

        [Fact]
        private async void userCanSignInWithEmailAndPassword()
        {
            // Arrange
            string email = "hans.muster@mail.ch";
            string password = "testAccount01";
            string signInToken;

            // Act
            signInToken = await this.testee.SignInWithEmail(email, password);

            // Assert
            signInToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        private async void whenNotRegisteredUserCanNotSignIn()
        {
            // Arrange
            string email = "hans.muster@notRegistered.ch";
            string password = "testAccount01";

            // Act
            Func<Task> func = async () => { await this.testee.SignInWithEmail(email, password); ; };

            // Assert
            func.Should().Throw<UserNotRegisterdException>();
        }
    }
}