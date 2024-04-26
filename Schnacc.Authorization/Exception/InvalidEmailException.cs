namespace Schnacc.Authorization.Exception {
    public class InvalidEmailException : System.Exception, IFirebaseHandledException
    {
        internal InvalidEmailException(string message) : base(message)
        {
        }
    }
}