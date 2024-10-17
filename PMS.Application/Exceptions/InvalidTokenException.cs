using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class InvalidTokenException : ApplicationException
{
    public InvalidTokenException(string message = "Invalid token.", string errorCode = nameof(InvalidTokenException)) 
        : base(HttpStatusCode.Unauthorized, message, errorCode)
    {
    }
}