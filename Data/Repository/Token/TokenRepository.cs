using ControleDeAcesso.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControleDeAcesso.Data.Repository.Token
{
    public class TokenRepository : ITokenRepository
    {
        private static readonly HashSet<string> _revokedTokens = new();
        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateTokenAsync(AuthModel user, string secretKey, int expirationMinutes = 60)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<string?> GetUserIdFromTokenAsync(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false 
                }, out SecurityToken validatedToken);

                var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                return Task.FromResult(userIdClaim?.Value);
            }
            catch
            {
                return Task.FromResult<string?>(null);
            }
        }

        public Task<bool> IsTokenRevokedAsync(string token)
        {
            return Task.FromResult(_revokedTokens.Contains(token));
        }

        public Task RevokeTokenAsync(string token)
        {
            _revokedTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> ValidateTokenAsync(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if(_revokedTokens.Contains(token))
                {
                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            } catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}
