using System.Net;
using ApplicationException = Bonyan.Layer.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class InvalidTokenException : ApplicationException
{
    public InvalidTokenException(string message = "Invalid token.", string errorCode = nameof(InvalidTokenException)) 
        : base(HttpStatusCode.Unauthorized, message, errorCode)
    {
    }
}