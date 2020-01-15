namespace Schnacc.Authorization.Exception
{
    using System;

    public class UserNotRegisteredException : Exception, IFirebaseHandledException
    {
        internal UserNotRegisteredException(string message) : base(message)
        {
        }
    }
}
