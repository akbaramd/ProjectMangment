using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Tasks.Exceptions;

public class TaskNotFoundException : ApplicationException
{
    public TaskNotFoundException(string message = "The specified task was not found.", string errorCode = nameof(TaskNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}