using System.Collections.Generic;

namespace ProjectName.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId, IEnumerable<string> roles);
        bool ValidateToken(string token, out string userId);
    }
}