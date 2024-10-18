namespace PMS.Application.Exceptions;


public class MemberNotFoundException : Exception
{
    public MemberNotFoundException(string message) : base(message) { }
}