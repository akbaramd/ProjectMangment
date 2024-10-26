using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Projects.Exceptions;

public class ColumnNotFoundException : ApplicationException
{
    public ColumnNotFoundException(string message = "The specified column was not found.", string errorCode = nameof(ColumnNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}