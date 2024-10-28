using System.Net;
using ApplicationException = Bonyan.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Sprints.Exceptions;

public class ColumnNotFoundException : ApplicationException
{
    public ColumnNotFoundException(string message = "The specified column was not found.", string errorCode = nameof(ColumnNotFoundException)) 
        : base(HttpStatusCode.NotFound, message, errorCode)
    {
    }
}