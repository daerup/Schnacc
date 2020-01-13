using static System.Exception;

namespace Schnacc.Authorization.Exception
{
    using System;

    public class UserNotRegisterdException : Exception, IFirebaseHandledException
    {
        internal UserNotRegisterdException(string message) : base(message)
        {
        }
    }
}
