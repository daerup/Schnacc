namespace Schnacc.Authorization.Exception {
    internal class InvalidEmailException : System.Exception, IFirebaseHandledException
    {
        internal InvalidEmailException(string message) : base(message)
        {
        }
    }
}