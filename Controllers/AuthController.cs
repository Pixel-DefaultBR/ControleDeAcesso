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
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenRepository _tokenRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, ITokenRepository tokenRepository, IConfiguration configuration, IMapper mapper)
    {
        _authService = authService;
        _tokenRepository = tokenRepository;
        _configuration = configuration;
        _mapper = mapper;
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

        var loginResponse = _mapper.Map<LoginResponseDto>(result.Value);

        return Ok(loginResponse);
    }
}
