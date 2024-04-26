namespace Schnacc.Authorization.Exception {
    public class WrongLoginCredentialsException : System.Exception, IFirebaseHandledException
    {
        internal WrongLoginCredentialsException(string message) : base(message)
        {
        }
    }
}