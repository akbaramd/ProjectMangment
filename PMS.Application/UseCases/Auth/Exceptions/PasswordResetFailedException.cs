using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class PasswordResetFailedException : ApplicationException
{
    public PasswordResetFailedException(string message = "Password reset failed.", string errorCode = nameof(PasswordResetFailedException)) 
        : base(HttpStatusCode.BadRequest, message, errorCode)
    {
    }
}