using static System.Exception;

namespace Schnacc.Authorization.Exception
{
    using System;

    public class UserNotRegisterdException : Exception
    {
        public UserNotRegisterdException(string message) : base(message)
        {
        }
    }
}
