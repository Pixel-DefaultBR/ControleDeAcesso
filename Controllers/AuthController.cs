using AutoMapper;
using ControleDeAcesso.Data.Repository;
using ControleDeAcesso.Data.Repository.Auth;
using ControleDeAcesso.Data.Repository.Token;
using ControleDeAcesso.DTOS;
using ControleDeAcesso.Model;
using ControleDeAcesso.Model.Response;
using ControleDeAcesso.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITwoFactorTokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, ITwoFactorTokenService tokenService ,IConfiguration configuration, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Dados inválidos para registro.");
        }

        var model = _mapper.Map<AuthModel>(user);

        var result = await _authService.CreateUserAsync(model);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage ?? "Erro ao registrar usuário.");
        }

        var response = _mapper.Map<RegisterResponseDto>(result.Value);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);
        
        if (!result.IsSuccess)
        {
            return Unauthorized(result.ErrorMessage ?? "Email ou senha inválidos.");
        }

        var token = await _tokenService.GetTokenAsync(result.Value.Email);

        var loginResponse = _mapper.Map<LoginResponseDto>(result.Value);
        loginResponse.Message = "Login válido. Verifique o código enviado por e-mail.";

        return Ok(loginResponse);
    }

    [HttpPost("verify2FA")]
    public async Task<IActionResult> Verify2FA([FromBody] Verify2FARequestDto request)
    {
        var result = await _authService.Verify2FAAsync(request.Guid, request.VerificationCode);

        if (!result.IsSuccess)
            return Unauthorized(result.ErrorMessage);

        var verifyResponse = _mapper.Map<Verify2FAResponseDto>(result.Value);
        return Ok(verifyResponse);
    }

}
