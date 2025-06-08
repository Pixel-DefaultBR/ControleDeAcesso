namespace ControleDeAcesso.DTOS
{
    public class Verify2FARequestDto
    {
        public string Guid { get; set; }
        public string VerificationCode { get; set; }
        public Verify2FARequestDto(string guid, string verificationCode)
        {
            Guid = guid;
            VerificationCode = verificationCode;
        }
        public Verify2FARequestDto() { }
    }
}
