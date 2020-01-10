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
        private void userCanSignInWithEmailAndPassword()
        {
            // Arrange
            string email = "hans.muster@mail.ch";
            string password = "testAccount01";
            string signInToken;

            // Act
            signInToken = this.testee.SignInWithEmail(email, password);

            // Assert
            signInToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        private void whenNotRegisteredUserCanNotSignIn()
        {
            // Arrange
            string email = "hans.muster@notRegistered.ch";
            string password = "testAccount01";

            // Act
            Action act = () => this.testee.SignInWithEmail(email, password);

            // Assert
            act.Should().Throw<UserNotRegisterdException>();
        }
    }
}