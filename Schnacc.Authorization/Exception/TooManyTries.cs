namespace Schnacc.Authorization.Exception {
    public class TooManyTries : System.Exception, IFirebaseHandledException
    {
        internal TooManyTries(string message) : base(message)
        {
        }
    }
}