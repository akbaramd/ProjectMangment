using System.Net;
using ApplicationException = SharedKernel.DomainDrivenDesign.Application.Exceptions.ApplicationException;

namespace PMS.Application.UseCases.Boards.Exceptions
{
    public class BoardNotFoundException : ApplicationException
    {
        public BoardNotFoundException(string message = "Board not found.", string errorCode = nameof(BoardNotFoundException)) 
            : base(HttpStatusCode.NotFound, message, errorCode)
        {
        }
    }
}