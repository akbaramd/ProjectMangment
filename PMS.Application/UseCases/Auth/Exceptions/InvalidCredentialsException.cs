using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException(string message = "Invalid credentials.", string errorCode = nameof(InvalidCredentialsException)) 
        : base(HttpStatusCode.Unauthorized, message, errorCode)
    {
    }
}