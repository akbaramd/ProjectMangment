using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class EmailNotConfirmedException : ApplicationException
{
    public EmailNotConfirmedException(string message = "Email address not confirmed.", string errorCode = nameof(EmailNotConfirmedException)) 
        : base(HttpStatusCode.Forbidden, message, errorCode)
    {
    }
}