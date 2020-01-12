﻿using System.Threading.Tasks;

namespace Schnacc.Authorization
{
    using System;
    using Firebase.Auth;

    using Exception;

    public class AuthorizationApi
    {
        private readonly FirebaseAuthProvider authProvider;
        private User loggedInUser;
        public AuthorizationApi()
        {
            this.authProvider = new FirebaseAuthProvider(new FirebaseConfig(AuthConfig.ApiKey));
        }

        public void RegisterWithEmail(string email, string password, string displayName)
        {
            FirebaseAuthLink userWithEmailAndPasswordAsync = this.authProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName).Result;
            this.authProvider.SendEmailVerificationAsync(userWithEmailAndPasswordAsync);
        }

        public async Task<string> SignInWithEmail(string email, string password)
        {
            try
            {
                FirebaseAuthLink authLink = (await this.authProvider.SignInWithEmailAndPasswordAsync(email, password));
                this.loggedInUser = authLink.User;
                return authLink.FirebaseToken;
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.UnknownEmailAddress)
            {
                throw new UserNotRegisterdException($"There is no user in this database with the {email}. Please register first");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.WrongPassword)
            {
                throw new WrongLoginCredentials($"Your login credentials are incorrect :/");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.InvalidEmailAddress)
            {
                throw new InvalidEmail($"Are you kidding me? '{email}' is not an email...");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.TooManyAttemptsTryLater)
            {
                throw new TooManyTries($"Chill my dude, you are doing too much. Try again later");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public bool userHasVerifiedEmail()
        {
            return this.loggedInUser.IsEmailVerified;
        }
    }
}
