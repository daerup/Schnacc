namespace Schnacc.Authorization.Exception {
    public class TooManyTriesException : System.Exception, IFirebaseHandledException
    {
        public TooManyTriesException(string message) : base(message)
        {
        }
    }
}