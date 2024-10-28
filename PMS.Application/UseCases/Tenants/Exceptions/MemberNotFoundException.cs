namespace PMS.Application.UseCases.Tenants.Exceptions;


public class MemberNotFoundException : Exception
{
    public MemberNotFoundException(string message) : base(message) { }
}