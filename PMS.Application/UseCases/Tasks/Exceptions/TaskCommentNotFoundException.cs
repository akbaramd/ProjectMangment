using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Tasks.Exceptions;

public class TaskCommentNotFoundException : ApplicationException
{
    public TaskCommentNotFoundException(string message = "The specified comment was not found.", string errorCode = nameof(TaskCommentNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}