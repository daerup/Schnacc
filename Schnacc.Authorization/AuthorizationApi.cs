namespace Schnacc.Authorization
{
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Firebase.Auth;

    using Schnacc.Authorization.Exception;

    public class AuthorizationApi
    {
        private readonly FirebaseAuthProvider authProvider;

        public AuthorizationApi()
        {
            this.authProvider = new FirebaseAuthProvider(new FirebaseConfig(AuthConfig.ApiKey));
        }

        public void RegisterWithEmail(string email, string password, string displayName)
        {
            FirebaseAuthLink userWithEmailAndPasswordAsync = this.authProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName).Result;
            this.authProvider.SendEmailVerificationAsync(userWithEmailAndPasswordAsync);
        }

        public string SignInWithEmail(string email, string password)
        {
            try
            {
                return this.authProvider.SignInWithEmailAndPasswordAsync(email, password).Result.FirebaseToken;
            }
            catch (AggregateException e) when ((e.InnerException as FirebaseAuthException).Reason == AuthErrorReason.UnknownEmailAddress)
            {
                throw new UserNotRegisterdException($"There is no user in this database with the {email}. Please register first");
            }
        }

        //public string SignInAnonymously()
        //{
        //    return this.authProvider.SignInAnonymouslyAsync().Result.FirebaseToken;
        //}
    }
}
