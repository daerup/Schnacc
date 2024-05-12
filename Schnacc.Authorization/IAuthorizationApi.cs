using System.Threading.Tasks;

namespace Schnacc.Authorization
{
    public interface IAuthorizationApi
    {
        public string Username { get; }
        public bool IsAnonymous { get; }
        public bool EmailIsVerified { get; }
        Task RegisterWithEmail(string email, string password, string displayName);
        Task<string> SignInWithEmail(string email, string password);
        Task<string> SignInAnonymous();
        void SignOut();
    }
}