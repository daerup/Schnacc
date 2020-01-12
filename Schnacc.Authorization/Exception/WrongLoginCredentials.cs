namespace Schnacc.Authorization.Exception {
    internal class WrongLoginCredentials : System.Exception, IFirebaseHandledException
    {
        internal WrongLoginCredentials(string message) : base(message)
        {
        }
    }
}