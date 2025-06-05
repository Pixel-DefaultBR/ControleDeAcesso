using ControleDeAcesso.Model;
using System.Threading.Tasks;

namespace ControleDeAcesso.Data.Repository.Token
{
    public interface ITokenRepository
    {
        Task<string> GenerateTokenAsync(AuthModel user, string secretKey, int expirationMinutes = 60);
        Task<bool> ValidateTokenAsync(string token, string secretKey);
        Task<string?> GetUserIdFromTokenAsync(string token, string secretKey);
        Task RevokeTokenAsync(string token);
        Task<bool> IsTokenRevokedAsync(string token);
        Task CommitAsync();
    }
}
