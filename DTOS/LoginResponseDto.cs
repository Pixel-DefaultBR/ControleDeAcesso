using System.Text.Json.Serialization;

namespace ControleDeAcesso.DTOS
{
    public class LoginResponseDto
    {
        [JsonPropertyName("guiid")]
        public string PreAuthToken { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
