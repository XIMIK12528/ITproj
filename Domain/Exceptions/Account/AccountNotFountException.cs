using Domain.Exceptions.Base;

namespace Domain.Exceptions.Account
{
    public class AccountNotFountException : NotFoundException
    {
        public override string ResponseMessage => "Account not found";

        public AccountNotFountException(string login, Exception? innerException = null)
            : base($"Account not found. Login: {login}", innerException)
        {
        }
    }
}
