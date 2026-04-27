using System.Net;

namespace Domain.Exceptions.Base
{
    public abstract class BaseProjException : Exception
    {
        public abstract HttpStatusCode Code { get; }

        public int ResponseCode => (int)Code;
        public abstract string ResponseMessage { get; }

        public BaseProjException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
