using Schnacc.Authorization.Exception;

namespace Schnacc.Authorization {
    public class UndifinedException : System.Exception, IFirebaseHandledException 
    {
        public UndifinedException(string s) :base(s)
        {
        }
    }
}