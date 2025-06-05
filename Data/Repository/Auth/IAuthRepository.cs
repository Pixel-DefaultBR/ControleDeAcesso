using ControleDeAcesso.Model;

namespace ControleDeAcesso.Data.Repository.Auth
{
    public interface IAuthRepository
    {
        Task<AuthModel> GetUserByEmailAsync(string email);
        Task<AuthModel> GetUserByIdAsync(int id);
        Task<AuthModel> CreateUserAsync(AuthModel user);
        Task<AuthModel> UpdateUserAsync(int id, AuthModel user);
        Task DeleteUserAsync(int id);
        Task CommitAsync();
    }
}
