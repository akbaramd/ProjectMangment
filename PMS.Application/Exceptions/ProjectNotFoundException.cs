using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.Exceptions
{
    public class ProjectNotFoundException : ApplicationException
    {
        public ProjectNotFoundException(string message = "The specified project was not found.", string errorCode = nameof(ProjectNotFoundException)) 
            : base(HttpStatusCode.NotFound, message, errorCode)
        {
        }
    }
}