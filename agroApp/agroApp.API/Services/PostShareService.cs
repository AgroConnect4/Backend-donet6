using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Http; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace agroApp.API.Services
{
    public class PostShareService : IPostShareService
    {
        private readonly IPostShareRepository _shareRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostShareService> _logger;

        public PostShareService(IPostShareRepository shareRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<PostShareService> logger)
        {
            _shareRepository = shareRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

private Guid GetUserIdFromToken()
{
    var httpContextAccessor = _httpContextAccessor;
    var configuration = _configuration;
    var logger = _logger;

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
            ClockSkew = TimeSpan.Zero //Important for token validation
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

        if (Guid.TryParse(userIdClaim.Value, out Guid userId))
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
        throw new UnauthorizedAccessException("Invalid JWT token."); //Re-throw or handle differently.
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unexpected error retrieving User ID from JWT token.");
        throw; //Re-throw the exception
    }
}

        public async Task<Guid> SharePostAsync(SharePostDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Os dados do compartilhamento não podem ser nulos.");
            }

            Guid userId = GetUserIdFromToken(); //Directly get Guid

            var share = new PostShare
            {
                PostId = request.PostId,
                UserId = userId, 
                SharedAt = DateTime.UtcNow
            };

            await _shareRepository.AddAsync(share);
            return share.Id;
        }

        public async Task<List<PostShare>> GetSharesByPostIdAsync(Guid postId)
        {
            return await _shareRepository.GetSharesByPostIdAsync(postId);
        }

        public async Task<PostShare> UpdateShareAsync(Guid shareId, SharePostDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Os dados da atualização do compartilhamento não podem ser nulos.");
            }

            var share = await _shareRepository.GetByIdAsync(shareId);
            if (share == null)
            {
                throw new KeyNotFoundException("Compartilhamento não encontrado.");
            }

            // Atualize as propriedades do compartilhamento, se necessário
            // ...

            return await _shareRepository.UpdateAsync(share);
        }

        public async Task DeleteShareAsync(Guid shareId)
        {
            await _shareRepository.DeleteAsync(shareId);
        }

        public async Task<PostShare> GetShareByIdAsync(Guid shareId)
        {
            var share = await _shareRepository.GetByIdAsync(shareId);
            return share; 
        }
    }
}