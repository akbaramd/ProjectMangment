using System.Net;
using ApplicationException = Bonyan.Layer.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class EmailNotConfirmedException : ApplicationException
{
    public EmailNotConfirmedException(string message = "Email address not confirmed.", string errorCode = nameof(EmailNotConfirmedException)) 
        : base(HttpStatusCode.Forbidden, message, errorCode)
    {
    }
}