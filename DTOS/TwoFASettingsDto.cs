namespace ControleDeAcesso.DTOS
{
    public class TwoFASettingsDto
    {
        public int MaxAttempts { get; set; }
        public int LockoutDuration { get; set; }
    }
}
