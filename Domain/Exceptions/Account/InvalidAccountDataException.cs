using Domain.Exceptions.Base;
using System.Net;

namespace Domain.Exceptions.Account
{
    public class InvalidAccountDataException : InvalidException
    {
        public override string ResponseMessage => "Wrong password";

        public InvalidAccountDataException(string login, Exception? innerException = null)
            : base($"Invalid account data. Login: {login}", innerException)
        {
        }   
    }
}
