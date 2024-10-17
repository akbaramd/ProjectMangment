using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions;

public class UnauthorizedAccessException : ApplicationException
{
    public UnauthorizedAccessException(string message = "Unauthorized access.", string errorCode = nameof(UnauthorizedAccessException)) 
        : base(HttpStatusCode.Forbidden, message, errorCode) // Forbidden status code 403
    {
    }
}