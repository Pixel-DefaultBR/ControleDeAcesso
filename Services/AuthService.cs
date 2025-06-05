using ControleDeAcesso.Data.Repository.Auth;
using ControleDeAcesso.Data.Repository.Token;
using ControleDeAcesso.Model;
using ControleDeAcesso.Model.Response;
using Microsoft.AspNetCore.Identity;


namespace ControleDeAcesso.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;

        public AuthService(IAuthRepository authRepository, ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _secretKey = _configuration["Jwt:ChaveSuperSecretaEver"];
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

            string token = await _tokenRepository.GenerateTokenAsync(userResult, _secretKey);
            userResult.Token = token;

            return Result<AuthModel>.Success(userResult);
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
