using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Auth.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        // Constructor with default message and error code
        public UnauthorizedException(string message = "Unauthorized access.", string errorCode = nameof(UnauthorizedException)) 
            : base(HttpStatusCode.Unauthorized, message, errorCode)
        {
        }
    }
}