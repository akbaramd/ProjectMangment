

using System.Net;
using ApplicationException = Bonyan.Layer.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.User.Exceptions;

public class UserAlreadyRegisteredException : ApplicationException
{
    public UserAlreadyRegisteredException(string message = "UserEntity is already registered.", string errorCode = nameof(UserAlreadyRegisteredException)) 
        : base(HttpStatusCode.Conflict, message, errorCode) // Conflict status code 409
    {
    }
}