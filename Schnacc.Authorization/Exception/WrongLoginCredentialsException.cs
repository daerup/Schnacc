namespace Schnacc.Authorization.Exception {
    public class WrongLoginCredentialsException : System.Exception, IFirebaseHandledException
    {
        public WrongLoginCredentialsException(string message) : base(message)
        {
        }
    }
}