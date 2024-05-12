using System.Threading.Tasks;
using Firebase.Auth;
using Schnacc.Authorization.Exception;

namespace Schnacc.Authorization
{
    public class AuthorizationApi
    {
        private readonly AuthConfig _config;
        private readonly FirebaseAuthProvider _authProvider;
        private string AccessToken { get; set; }
        public string Username { get; private set; }

        public bool EmailIsVerified =>
            this._authProvider.GetUserAsync(this.AccessToken).GetAwaiter().GetResult().IsEmailVerified;

        public AuthorizationApi(AuthConfig config)
        {
            this._config = config;
            this._authProvider = new FirebaseAuthProvider(new FirebaseConfig(this._config.ApiKey));
        }

        public async Task RegisterWithEmail(string email, string password, string displayName)
        {
            try
            {
                var link =
                    await this._authProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName, true);
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.EmailExists)
            {
                throw new UserAlreadyRegisteredException(
                    "There is already a user with this email registered, try another one or Login");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.WeakPassword)
            {
                throw new PasswordTooWeakException("Yikes, your password is too weak...");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.InvalidEmailAddress)
            {
                throw new InvalidEmailException($"Are you kidding me? '{email}' is not an email...");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.Undefined)
            {
                throw new UndefinedException("Looks like there was an Error. Probably your Internet Connection");
            }
        }

        public async Task<string> SignInWithEmail(string email, string password)
        {
            try
            {
                var authLink = await this._authProvider.SignInWithEmailAndPasswordAsync(email, password);
                this.AccessToken = authLink.FirebaseToken;
                this.Username = authLink.User.DisplayName;
                return this.AccessToken;
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.UnknownEmailAddress)
            {
                throw new UserNotRegisteredException(
                    $"There is no user in this database with the {email}. Please register first");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.WrongPassword)
            {
                throw new WrongLoginCredentialsException("Your login credentials are incorrect...");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.InvalidEmailAddress)
            {
                throw new InvalidEmailException($"Are you kidding me? '{email}' is not an email...");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.TooManyAttemptsTryLater)
            {
                throw new TooManyTriesException("Chill my dude, you are doing too much. Try again later");
            }
            catch(FirebaseAuthException e) when(e.Reason == AuthErrorReason.Undefined)
            {
                throw new UndefinedException("Looks like there was an Error. Probably your Internet Connection");
            }
        }

        public async Task<string> SignInAnonymous()
        {
            var authLink = await this._authProvider.SignInAnonymouslyAsync();
            this.AccessToken = authLink.FirebaseToken;
            return this.AccessToken;
        }
    }
}
