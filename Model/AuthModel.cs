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
        public DateTime TokenExpiration { get; set; }
    }
}
