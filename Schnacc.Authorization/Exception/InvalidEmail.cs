namespace Schnacc.Authorization.Exception {
    internal class InvalidEmail : System.Exception, IFirebaseHandledException
    {
        internal InvalidEmail(string message) : base(message)
        {
        }
    }
}