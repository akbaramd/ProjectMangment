using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions
{
    public class TenantNotFoundException : ApplicationException
    {
        public TenantNotFoundException(string message = "Tenant not found.", string errorCode = nameof(TenantNotFoundException)) 
            : base(HttpStatusCode.NotFound, message, errorCode)
        {
        }
    }
}