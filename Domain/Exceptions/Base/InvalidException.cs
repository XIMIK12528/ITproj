using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Exceptions.Base
{
    public abstract class InvalidException : BaseProjException
    {
        public override HttpStatusCode Code => HttpStatusCode.BadRequest;

        protected InvalidException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
