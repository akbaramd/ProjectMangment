

using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.User.Exceptions;

public class UserAlreadyRegisteredException : ApplicationException
{
    public UserAlreadyRegisteredException(string message = "User is already registered.", string errorCode = nameof(UserAlreadyRegisteredException)) 
        : base(HttpStatusCode.Conflict, message, errorCode) // Conflict status code 409
    {
    }
}