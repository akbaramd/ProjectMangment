using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException(string message = "Invalid credentials.", string errorCode = nameof(InvalidCredentialsException)) 
        : base(HttpStatusCode.Unauthorized, message, errorCode)
    {
    }
}