namespace Schnacc.Authorization.Exception
{
    public class UserNotRegisteredException : System.Exception, IFirebaseHandledException
    {
        internal UserNotRegisteredException(string message) : base(message)
        {
        }
    }
}
