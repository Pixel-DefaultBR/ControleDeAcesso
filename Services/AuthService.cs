using ControleDeAcesso.Data.Repository.Auth;
using ControleDeAcesso.Data.Repository.Token;
using ControleDeAcesso.DTOS;
using ControleDeAcesso.Model;
using ControleDeAcesso.Model.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace ControleDeAcesso.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITwoFactorTokenService _tokenService;
        private readonly string _secretKey;
        private readonly TwoFASettingsDto _twoFASettings;


        public AuthService(IAuthRepository authRepository,
            ITokenRepository tokenRepository, IConfiguration configuration, IEmailService emailService, ITwoFactorTokenService tokenService, IOptions<TwoFASettingsDto> twoFASettings)
        {
            _authRepository = authRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _secretKey = _configuration["Jwt:ChaveSuperSecretaEver"];
            _emailService = emailService;
            _tokenService = tokenService;
            _twoFASettings = twoFASettings.Value;
        }

        public async Task CommitAsync()
        {
            await _authRepository.CommitAsync();
        }

        public async Task<Result<AuthModel>> CreateUserAsync(AuthModel user)
        {
            if (user == null)
            {
                return Result<AuthModel>.Failure("Usuário não pode ser nulo.");
            }

            var existingUser = await _authRepository.GetUserByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return Result<AuthModel>.Failure("Email já está em uso.");
            }

            var passwordHasher = new PasswordHasher<AuthModel>();

            user.Password = passwordHasher.HashPassword(user, user.Password);
            user.Token = await _tokenRepository.GenerateTokenAsync(user, _secretKey);

            await _authRepository.CreateUserAsync(user);
            await _authRepository.CommitAsync();
            return Result<AuthModel>.Success(user); ;
        }

        public async Task<Result<AuthModel>> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<AuthModel>> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result<AuthModel>.Failure("Email não pode ser nulo ou vazio.");
            }

            var user = await _authRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return Result<AuthModel>.Failure("Usuário não encontrado.");
            }

            return Result<AuthModel>.Success(user);
        }

        public async Task<Result<AuthModel>> GetUserByIdAsync(int id)
        {
            var user = await _authRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return Result<AuthModel>.Failure("Usuário não encontrado.");
            }
            return Result<AuthModel>.Success(user);
        }

        public async Task<Result<AuthModel>> LoginAsync(string email, string password)
        {
            var userResult = await _authRepository.GetUserByEmailAsync(email);

            if (string.IsNullOrEmpty(_secretKey))
            {
                return Result<AuthModel>.Failure("Erro do nosso lado.");
            }
            if (userResult == null)
            {
                return Result<AuthModel>.Failure("Usuário não encontrado.");
            }

            var passwordHasher = new PasswordHasher<AuthModel>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userResult, userResult.Password, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Result<AuthModel>.Failure("Senha incorreta.");
            }

            var preAuthToken = Guid.NewGuid().ToString();

            userResult.PreAuthToken = preAuthToken;
            userResult.PreAuthTokenExpiration = DateTime.UtcNow.AddMinutes(5);

            await _authRepository.UpdateUserAsync(userResult.Id, userResult);
            await _authRepository.CommitAsync();

            await _emailService.SendEmailAsync(
                to: "voce",
                subject: "Código de Verificação 2FA",
                body: $"Seu código de verificação é: {userResult.VerificationCode}",
                model: userResult
                );

            return Result<AuthModel>.Success(userResult);
        }

        public async Task<Result<AuthModel>> Verify2FAAsync(string guid, string verificationCode)
        {
            var user = await _authRepository.GetUserByPreAuthTokenIdAsync(guid);

            if (user == null)
            {
                return Result<AuthModel>.Failure("Usuário não encontrado.");
            }          
            if (user.TwoFABlockedUntil.HasValue && user.TwoFABlockedUntil > DateTime.UtcNow)
            {
                return Result<AuthModel>.Failure("Usuário temporariamente bloqueado. Tente novamente mais tarde.");
            }
            if (user.PreAuthTokenExpiration < DateTime.UtcNow)
            {
                user.PreAuthToken = null;
                user.PreAuthTokenExpiration = null;
                return Result<AuthModel>.Failure("Token expirado.");
            }
            if (user.VerificationCode != verificationCode || user.VerificationCodeExpiration < DateTime.UtcNow)
            {
                user.Failed2FAAttempts++;

                if (user.Failed2FAAttempts >= _twoFASettings.MaxAttempts)
                {
                    user.TwoFABlockedUntil = DateTime.UtcNow.AddMinutes(_twoFASettings.LockoutDuration); 
                    user.Failed2FAAttempts = 0; 
                }

                await _authRepository.UpdateUserAsync(user.Id, user);
                await _authRepository.CommitAsync();

                return Result<AuthModel>.Failure("Código inválido ou expirado.");
            }

            user.VerificationCode = null;
            user.VerificationCodeExpiration = null;
            user.PreAuthToken = null;
            user.PreAuthTokenExpiration = null;
            user.Failed2FAAttempts = 0;
            user.TwoFABlockedUntil = null;

            await _authRepository.UpdateUserAsync(user.Id, user);
            await _authRepository.CommitAsync();

            string token = await _tokenRepository.GenerateTokenAsync(user, _secretKey);
            user.Token = token;

            return Result<AuthModel>.Success(user);
        }


        public async Task<Result<AuthModel>> UpdateUserAsync(int id, AuthModel user)
        {
            var existingUser = await _authRepository.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                return Result<AuthModel>.Failure("Usuário não encontrado.");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;

            await _authRepository.UpdateUserAsync(id, existingUser);
            await _authRepository.CommitAsync();

            return Result<AuthModel>.Success(existingUser);
        }
    }
}
