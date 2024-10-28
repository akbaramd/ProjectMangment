using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Tenants.Exceptions
{
    public class TenantNotFoundException : ApplicationException
    {
        public TenantNotFoundException(string message = "Tenants not found.", string errorCode = nameof(TenantNotFoundException)) 
            : base(HttpStatusCode.NotFound, message, errorCode)
        {
        }
    }
}