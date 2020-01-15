using Schnacc.Authorization.Exception;

namespace Schnacc.Authorization {
    public class UserAlreadyRegisteredException : System.Exception, IFirebaseHandledException
    {
        public UserAlreadyRegisteredException(string message) : base(message)
        {
        }
    }
}