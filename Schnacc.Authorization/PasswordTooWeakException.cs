using Schnacc.Authorization.Exception;

namespace Schnacc.Authorization {
    public class PasswordTooWeakException : System.Exception, IFirebaseHandledException
    {
        public PasswordTooWeakException(string message) : base(message)
        {
        }
    }
}