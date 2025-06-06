namespace ControleDeAcesso.DTOS
{
    public class Verify2FARequestDto
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public Verify2FARequestDto(string email, string verificationCode)
        {
            Email = email;
            VerificationCode = verificationCode;
        }
        public Verify2FARequestDto() { }
    }
}
