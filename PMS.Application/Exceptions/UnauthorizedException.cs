using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions
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