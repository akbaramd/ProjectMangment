using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Sprints.Exceptions
{
    public class SprintNotFoundException : ApplicationException
    {
        public SprintNotFoundException(string message = "Sprint not found.", string errorCode = nameof(SprintNotFoundException)) 
            : base(HttpStatusCode.NotFound, message, errorCode)
        {
        }
    }
}