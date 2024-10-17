using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class TokenExpiredException : ApplicationException
{
    public TokenExpiredException(string message = "Token has expired.", string errorCode = nameof(TokenExpiredException)) 
        : base(HttpStatusCode.Unauthorized, message, errorCode)
    {
    }
}