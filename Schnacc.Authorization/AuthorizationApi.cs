namespace Schnacc.Authorization
{
    using System.Threading.Tasks;

    using Firebase.Auth;

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
            return this.authProvider.SignInWithEmailAndPasswordAsync(email, password).Result.FirebaseToken;
        }

        public string SignInAnonymously()
        {
            return this.authProvider.SignInAnonymouslyAsync().Result.FirebaseToken;
        }
    }
}
