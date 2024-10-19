namespace PMS.Application.Interfaces
{
    public interface ISmsService
    {
        System.Threading.Tasks.Task SendSmsAsync(string phoneNumber, string message);
    }
}