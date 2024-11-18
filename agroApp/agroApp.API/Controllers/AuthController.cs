using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using agroApp.Infra.Data.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace agroApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<User> userManager, IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userManager = userManager;
            _userRepository = userRepository; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); //Add model validation

            if (!_authService.IsValidEmail(registerDto.Email)) return BadRequest("Invalid email format.");

            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null) return BadRequest("Email already in use.");

            var result = await _authService.RegisterAsync(registerDto.Username, registerDto.Email, registerDto.Password);

            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest("Registration failed.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            // Encontre o usuário pelo email
            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

            if (result.Succeeded)
            {
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                var token = await _authService.GenerateTokenAsync(user);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(forgotPasswordDto.Email))
            {
                return BadRequest("Email é obrigatório.");
            }

            try
            {
                await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
                return Ok("Instruções de recuperação de senha enviadas.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}