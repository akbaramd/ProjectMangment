using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class AccountLockedException : ApplicationException
{
    public AccountLockedException(string message = "The account is locked.", string errorCode = nameof(AccountLockedException)) 
        : base(HttpStatusCode.Forbidden, message, errorCode)
    {
    }
}