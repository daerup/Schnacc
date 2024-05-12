using System.Threading.Tasks;

namespace Schnacc.Authorization
{
    public class OfflineAuthorizationApi : IAuthorizationApi
    {
        public string Username => "Offline User";
        public bool IsAnonymous => true;
        public bool EmailIsVerified => false;

        public Task RegisterWithEmail(string email, string password, string displayName) => throw new UndefinedException("Online features are not available in offline mode");

        public Task<string> SignInWithEmail(string email, string password) => throw new UndefinedException("Online features are not available in offline mode");

        public Task<string> SignInAnonymous() => throw new UndefinedException("Online features are not available in offline mode");

        public void SignOut()
        {
        }
    }
}