using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Http; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace agroApp.API.Services
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostCommentService> _logger; 

        public PostCommentService(IPostCommentRepository commentRepository, 
            IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, 
            IConfiguration configuration,
            ILogger<PostCommentService> logger)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        private Guid GetUserIdFromToken()
{
    var httpContextAccessor = _httpContextAccessor; //Assuming you have this injected.
    var configuration = _configuration; // Assuming you have this injected.
    var logger = _logger; // Assuming you have this injected.

    if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
    {
        logger.LogError("HttpContextAccessor is null or HttpContext is null.");
        throw new InvalidOperationException("Cannot access HTTP context.");
    }

    var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
    if (!authorizationHeader.Any())
    {
        logger.LogError("Authorization header not found.");
        throw new UnauthorizedAccessException("Authorization header is missing.");
    }

    var token = authorizationHeader.FirstOrDefault().Split(" ").LastOrDefault();
    if (string.IsNullOrEmpty(token))
    {
        logger.LogError("JWT token not found in Authorization header.");
        throw new UnauthorizedAccessException("JWT token is missing.");
    }

    try
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            logger.LogError("User ID claim not found in JWT token.");
            throw new UnauthorizedAccessException("User ID claim is missing.");
        }

        Guid userId;
        if (Guid.TryParse(userIdClaim.Value, out userId))
        {
            return userId;
        }
        else
        {
            logger.LogError("Invalid User ID format in JWT token.");
            throw new UnauthorizedAccessException("Invalid User ID format in JWT token.");
        }
    }
    catch (SecurityTokenException ex)
    {
        logger.LogError(ex, "Error validating JWT token: {Message}", ex.Message);
        throw new UnauthorizedAccessException("Invalid JWT token.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unexpected error retrieving User ID from JWT token.");
        throw;
    }
}

        public async Task<Guid> CreateCommentAsync(CreatePostCommentDto request, Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString()); //Corrected
                if (user == null) throw new ArgumentException("Usuário não encontrado.");

                var comment = new PostComment { /* ... */ };
                await _commentRepository.AddAsync(comment);
                return comment.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PostComment: {Message}", ex.Message);
                throw; // Re-throw the exception to be handled by the controller
            }
        }

        public async Task<List<PostComment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await _commentRepository.GetCommentsByPostIdAsync(postId);
        }

    public async Task<PostComment> UpdateCommentAsync(Guid commentId, UpdatePostCommentDto request)
    {
        // Validação do request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Os dados da atualização do comentário não podem ser nulos.");
        }

        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment == null)
        {
            return null; 
        }

        comment.Content = request.Content;
        comment.UpdatedAt = DateTime.UtcNow; // Atualiza a data de atualização
        return await _commentRepository.UpdateAsync(comment);
    }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<PostComment> GetCommentByIdAsync(Guid commentId)
        {
            return await _commentRepository.GetByIdAsync(commentId);
        }
    }
}