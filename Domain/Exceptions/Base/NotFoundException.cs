using System.Net;

namespace Domain.Exceptions.Base
{
    public abstract class NotFoundException : BaseProjException
    {
        public override HttpStatusCode Code => HttpStatusCode.NotFound;

        protected NotFoundException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
