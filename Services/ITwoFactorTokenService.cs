using ControleDeAcesso.Model;
using ControleDeAcesso.Model.Response;

public interface ITwoFactorTokenService
{
    Task<TwoFactorTokenModel> SaveTokenAsync(string email);
    Task<TwoFactorTokenModel?> GetTokenAsync(string token);
}
