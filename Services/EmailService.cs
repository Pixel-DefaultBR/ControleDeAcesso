using ControleDeAcesso.Data.Repository.Auth;
using ControleDeAcesso.Model;

namespace ControleDeAcesso.Services
{
    public class EmailService : IEmailService
    {
        private readonly IAuthRepository _authRepository;

        public EmailService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task SendEmailAsync(string to, string subject, string body, AuthModel model)
        {
            var code = GenerateVerifcationCode();
            model.VerificationCode = code;

            await _authRepository.UpdateUserAsync(model.Id, model);
            await _authRepository.CommitAsync();

            Console.WriteLine($"[2FA] Código de verificação enviado para {to}: {code}");
        }

        private string GenerateVerifcationCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            return code;
        }
    }
}
