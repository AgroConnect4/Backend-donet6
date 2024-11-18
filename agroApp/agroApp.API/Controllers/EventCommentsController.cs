using agroApp.API.DTOs;
using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using agroApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace agroApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Autenticação obrigatória para todos os métodos
    public class EventCommentsController : ControllerBase
    {
        private readonly IEventCommentService _commentService;
        private readonly UserManager<User> _userManager; // Adicione UserManager
        private readonly IHttpContextAccessor _httpContextAccessor; // Inject IHttpContextAccessor
        private readonly IConfiguration _configuration; // Inject IConfiguration
        private readonly ILogger<EventCommentsController> _logger;

        public EventCommentsController(
            IEventCommentService commentService, 
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ILogger<EventCommentsController> logger)
        {
            _commentService = commentService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

        private Guid GetUserIdFromToken()
{
    var token = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();

    if (string.IsNullOrEmpty(token))
    {
        _logger.LogError("Token JWT não encontrado no cabeçalho de autorização.");
        throw new UnauthorizedAccessException("Token JWT ausente.");
    }

    try
    {
       var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            _logger.LogError("Claim de ID de usuário inválida ou ausente no token JWT.");
            throw new UnauthorizedAccessException("ID de usuário inválida no token.");
        }

        return userId;
    }
    catch (SecurityTokenException ex)
    {
        _logger.LogError(ex, "Erro ao validar o token JWT.");
        throw new UnauthorizedAccessException("Token JWT inválido.");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro inesperado ao recuperar o ID do usuário do token JWT.");
        throw;
    }
}

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateEventCommentDto request)
        {
            try
            {
                var userId = GetUserIdFromToken(); // Get the user ID from the token
                var commentId = await _commentService.CreateCommentAsync(request, userId); //Pass validated userId
                return CreatedAtAction(nameof(GetCommentById), new { id = commentId }, null);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Acesso não autorizado.");
                return Unauthorized(ex.Message); // Retorna a mensagem da exceção para melhor depuração
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("{eventId}/comments")]
        public async Task<ActionResult<List<EventComment>>> GetCommentsByEventId(Guid eventId)
        {
            var comments = await _commentService.GetCommentsByEventIdAsync(eventId);
            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<EventComment>> GetCommentById(Guid commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateEventCommentDto request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var comment = await _commentService.GetCommentByIdAsync(commentId);
                if (comment == null || comment.UserId != userId) return Unauthorized(); 

                await _commentService.UpdateCommentAsync(commentId, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment: {Message}", ex.Message);
                return StatusCode(500, "Erro ao atualizar comentário.");
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var comment = await _commentService.GetCommentByIdAsync(commentId);
                if (comment == null || comment.UserId != userId) return Unauthorized();

                await _commentService.DeleteCommentAsync(commentId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao excluir comentário.");
                    return StatusCode(500, "Erro ao excluir comentário.");
                }
        }
    }
}