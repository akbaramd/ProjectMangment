namespace PMS.Application.UseCases.Tenant.Exceptions;


public class MemberNotFoundException : Exception
{
    public MemberNotFoundException(string message) : base(message) { }
}