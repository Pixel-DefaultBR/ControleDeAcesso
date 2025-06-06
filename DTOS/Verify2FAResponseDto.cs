namespace ControleDeAcesso.DTOS
{
    public class Verify2FAResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
