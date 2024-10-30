using System.Net;
using ApplicationException = Bonyan.Layer.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class RegistrationFailedException : ApplicationException
{
    public RegistrationFailedException(string message = "UserEntity registration failed.", string errorCode = nameof(RegistrationFailedException)) 
        : base(HttpStatusCode.BadRequest, message, errorCode)
    {
    }
}