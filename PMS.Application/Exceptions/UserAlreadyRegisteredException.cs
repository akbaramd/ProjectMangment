using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class UserAlreadyRegisteredException : ApplicationException
{
    public UserAlreadyRegisteredException(string message = "User is already registered.", string errorCode = nameof(UserAlreadyRegisteredException)) 
        : base(HttpStatusCode.Conflict, message, errorCode) // Conflict status code 409
    {
    }
}