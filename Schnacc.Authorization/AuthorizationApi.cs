using System.Threading.Tasks;

namespace Schnacc.Authorization
{
    using Firebase.Auth;

    using Exception;

    public class AuthorizationApi
    {
        private readonly FirebaseAuthProvider authProvider;
        public string AccessToken { get; private set; }
        public string Username { get; private set; }

        public bool EmailIsVerified =>
            this.authProvider.GetUserAsync(this.AccessToken).GetAwaiter().GetResult().IsEmailVerified;

        public AuthorizationApi()
        {
            this.authProvider = new FirebaseAuthProvider(new FirebaseConfig(AuthConfig.ApiKey));
        }

        public  async Task RegisterWithEmail(string email, string password, string displayName)
        {
            try
            {
                FirebaseAuthLink registeredUserAuth = await this.authProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName, true);
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.EmailExists)
            {
                throw new UserAlreadyRegisteredException($"There is already a user with this email registered, try another one or Login");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.WeakPassword)
            {
                throw new PasswordTooWeakException($"Yikes, your password is too weak...");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.InvalidEmailAddress)
            {
                throw new InvalidEmailException($"Are you kidding me? '{email}' is not an email...");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<string> SignInWithEmail(string email, string password)
        {
            try
            {
                FirebaseAuthLink authLink = (await this.authProvider.SignInWithEmailAndPasswordAsync(email, password));
                this.AccessToken = authLink.FirebaseToken;
                this.Username = authLink.User.DisplayName;
                return this.AccessToken;
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.UnknownEmailAddress)
            {
                throw new UserNotRegisteredException($"There is no user in this database with the {email}. Please register first");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.WrongPassword)
            {
                throw new WrongLoginCredentialsException($"Your login credentials are incorrect...");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.InvalidEmailAddress)
            {
                throw new InvalidEmailException($"Are you kidding me? '{email}' is not an email...");
            }
            catch (FirebaseAuthException e) when (e.Reason == AuthErrorReason.TooManyAttemptsTryLater)
            {
                throw new TooManyTriesException($"Chill my dude, you are doing too much. Try again later");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
