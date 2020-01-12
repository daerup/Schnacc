using System.Threading.Tasks;

namespace Schnacc.Authorization
{
    using System;
    using Firebase.Auth;

    using Exception;

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

        public async Task<string> SignInWithEmail(string email, string password)
        {
            try
            {
                return (await this.authProvider.SignInWithEmailAndPasswordAsync(email, password)).FirebaseToken;
            }
            catch (AggregateException e) when ((e.InnerException as FirebaseAuthException).Reason ==
                                               AuthErrorReason.UnknownEmailAddress)
            {
                throw new UserNotRegisterdException(
                    $"There is no user in this database with the {email}. Please register first");
            }
            catch (AggregateException e) when ((e.InnerException as FirebaseAuthException).Reason ==
                                               AuthErrorReason.WrongPassword)
            {
                throw new WrongLoginCredentials($"Your login credentials are incorrect :/");
            }
            catch (AggregateException e) when ((e.InnerException as FirebaseAuthException).Reason ==
                                               AuthErrorReason.InvalidEmailAddress)
            {
                throw new InvalidEmail($"Are you kidding me? {email} is not an email...");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        //public string SignInAnonymously()
        //{
        //    return this.authProvider.SignInAnonymouslyAsync().Result.FirebaseToken;
        //}
    }
}
