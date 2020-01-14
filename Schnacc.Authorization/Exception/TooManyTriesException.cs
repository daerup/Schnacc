namespace Schnacc.Authorization.Exception {
    public class TooManyTriesException : System.Exception, IFirebaseHandledException
    {
        internal TooManyTriesException(string message) : base(message)
        {
        }
    }
}