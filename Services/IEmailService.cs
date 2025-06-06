using ControleDeAcesso.Model;

namespace ControleDeAcesso.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body, AuthModel model);
    }
}
