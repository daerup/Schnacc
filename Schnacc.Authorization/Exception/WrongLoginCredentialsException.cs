namespace Schnacc.Authorization.Exception {
    internal class WrongLoginCredentialsException : System.Exception, IFirebaseHandledException
    {
        internal WrongLoginCredentialsException(string message) : base(message)
        {
        }
    }
}