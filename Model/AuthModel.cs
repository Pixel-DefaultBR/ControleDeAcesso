using ControleDeAcesso.Model.Enum;

namespace ControleDeAcesso.Model
{
    public class AuthModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserEnum Role { get; set; } = UserEnum.User;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string? VerificationCode { get; set; } = string.Empty;
        public string ? PreAuthToken { get; set; } = string.Empty;
        public DateTime? VerificationCodeExpiration { get; set; } = null;
        public DateTime? PreAuthTokenExpiration { get; set; } = null;
        public DateTime TokenExpiration { get; set; }
        public int Failed2FAAttempts { get; set; } = 0;
        public DateTime? TwoFABlockedUntil { get; set; }
    }
}
