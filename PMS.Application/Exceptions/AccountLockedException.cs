using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class AccountLockedException : ApplicationException
{
    public AccountLockedException(string message = "The account is locked.", string errorCode = nameof(AccountLockedException)) 
        : base(HttpStatusCode.Forbidden, message, errorCode)
    {
    }
}