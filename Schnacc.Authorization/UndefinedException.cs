using Schnacc.Authorization.Exception;

namespace Schnacc.Authorization {
    public class UndefinedException : System.Exception, IFirebaseHandledException 
    {
        public UndefinedException(string s) :base(s)
        {
        }
    }
}