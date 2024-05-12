namespace Schnacc.Authorization.Exception {
    public class InvalidEmailException : System.Exception, IFirebaseHandledException
    {
        public InvalidEmailException(string message) : base(message)
        {
        }
    }
}