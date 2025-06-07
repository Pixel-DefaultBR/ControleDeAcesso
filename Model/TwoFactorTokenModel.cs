namespace ControleDeAcesso.Model
{
    public class TwoFactorTokenModel
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
