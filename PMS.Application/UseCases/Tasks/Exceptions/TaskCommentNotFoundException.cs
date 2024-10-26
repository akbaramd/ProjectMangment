using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Projects.Exceptions;

public class TaskCommentNotFoundException : ApplicationException
{
    public TaskCommentNotFoundException(string message = "The specified comment was not found.", string errorCode = nameof(TaskCommentNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}