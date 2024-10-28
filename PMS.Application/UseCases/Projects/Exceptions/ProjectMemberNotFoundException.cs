using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Projects.Exceptions;

public class ProjectMemberNotFoundException : ApplicationException
{
    public ProjectMemberNotFoundException(string message = "The specified project member was not found.", string errorCode = nameof(ProjectMemberNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}