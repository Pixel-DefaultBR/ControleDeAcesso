using ControleDeAcesso.Model;
using ControleDeAcesso.Model.Response;

namespace ControleDeAcesso.Services
{
    public interface IAuthService
    {
        Task<Result<AuthModel>> GetUserByEmailAsync(string email);
        Task<Result<AuthModel>> GetUserByIdAsync(int id);
        Task<Result<AuthModel>> CreateUserAsync(AuthModel user);
        Task<Result<AuthModel>> UpdateUserAsync(int id, AuthModel user);
        Task<Result<AuthModel>> DeleteUserAsync(int id);

        Task<Result<AuthModel>> LoginAsync(string email, string password);
        Task<Result<AuthModel>> Verify2FAAsync(string email, string verificationCode);
        Task CommitAsync();
    }
}
