using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();

    }
}