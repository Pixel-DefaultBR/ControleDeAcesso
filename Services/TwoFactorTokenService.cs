using ControleDeAcesso.Model;
using ControleDeAcesso.Model.Response;
using System.Collections.Concurrent;

public class TwoFactorTokenService : ITwoFactorTokenService
{
    private readonly ConcurrentDictionary<string, TwoFactorTokenModel> _tokens = new();

    public Task<TwoFactorTokenModel> SaveTokenAsync(string email)
    {
        var tempToken = Guid.NewGuid().ToString();
        var tokenModel = new TwoFactorTokenModel
        {
            Token = tempToken,
            Email = email,
            Expiration = DateTime.UtcNow.AddMinutes(5)
        };

        _tokens[tempToken] = tokenModel;

        return Task.FromResult(tokenModel);
    }

    public Task<TwoFactorTokenModel?> GetTokenAsync(string token)
    {
        _tokens.TryGetValue(token, out var tokenModel);

        if (tokenModel == null || tokenModel.Expiration < DateTime.UtcNow)
        {
            _tokens.TryRemove(token, out _);
            return Task.FromResult<TwoFactorTokenModel?>(null);
        }

        return Task.FromResult(tokenModel);
    }
}
