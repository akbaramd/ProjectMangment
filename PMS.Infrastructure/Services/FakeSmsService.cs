using PMS.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace PMS.Infrastructure.Services
{
    public class FakeSmsService : ISmsService
    {
        private readonly ILogger<FakeSmsService> _logger;

        public FakeSmsService(ILogger<FakeSmsService> logger)
        {
            _logger = logger;
        }

        public System.Threading.Tasks.Task SendSmsAsync(string phoneNumber, string message)
        {
            // Simulate SMS sending by logging the SMS message
            _logger.LogInformation($"FAKE SMS sent to {phoneNumber}: {message}");
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}