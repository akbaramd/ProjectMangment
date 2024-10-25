using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions;

public class InvalidInvitationTokenException : ApplicationException
{
    public InvalidInvitationTokenException(string message = "Invalid invitation token.", string errorCode = nameof(InvalidInvitationTokenException)) 
        : base(HttpStatusCode.BadRequest, message, errorCode)
    {
    }
}