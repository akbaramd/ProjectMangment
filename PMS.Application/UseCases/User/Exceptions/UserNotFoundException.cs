using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.User.Exceptions;

public class UserNotFoundException : ApplicationException
{
    public UserNotFoundException(string message = "User not found.", string errorCode = nameof(UserNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}