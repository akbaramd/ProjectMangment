using System.Net;
using ApplicationException = Bonyan.Layer.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.User.Exceptions;

public class UserNotFoundException : ApplicationException
{
    public UserNotFoundException(string message = "UserEntity not found.", string errorCode = nameof(UserNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}