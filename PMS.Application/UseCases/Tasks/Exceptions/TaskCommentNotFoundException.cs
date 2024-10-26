using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Projects.Exceptions;

public class CommentNotFoundException : ApplicationException
{
    public CommentNotFoundException(string message = "The specified comment was not found.", string errorCode = nameof(CommentNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}