using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Http; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt; 
using Microsoft.IdentityModel.Tokens; 
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace agroApp.API.Services
{
    public class EventShareService : IEventShareService
    {
        private readonly IEventShareRepository _shareRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventShareService> _logger;

        public EventShareService(IEventShareRepository shareRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ILogger<EventShareService> logger)
        {
            _shareRepository = shareRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

        private string GetUserIdFromToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                throw new SecurityTokenException("Token JWT não encontrado."); //Explicit error handling
            }

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"], //Configure your Issuer
                    ValidAudience = _configuration["Jwt:Audience"], //Configure your Audience
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), //Configure your Key
                    ClockSkew = TimeSpan.Zero // Important for precise token expiry checks
                };


                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                return userIdClaim?.Value; 
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Erro ao validar o token JWT: {Message}", ex.Message);
                throw new UnauthorizedAccessException("Token JWT inválido."); //More informative exception
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao validar o token JWT.");
                throw; 
            }
        }

        public async Task<Guid> ShareEventAsync(ShareEventDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Os dados do compartilhamento não podem ser nulos.");
            }

            var userIdString = GetUserIdFromToken(); // Get UserId as string
            Guid userId;

            if(Guid.TryParse(userIdString, out userId))
            {
                var share = new EventShare
                {
                    EventId = request.EventId,
                    UserId = userId,
                    SharedAt = DateTime.UtcNow
                };

                await _shareRepository.AddAsync(share);
                return share.Id;
            }
            else
            {
                throw new ArgumentException("Invalid User ID in token.");
            }
        }

        public async Task<List<EventShare>> GetSharesByEventIdAsync(Guid eventId)
        {
            return await _shareRepository.GetSharesByEventIdAsync(eventId);
        }

        public async Task<EventShare> UpdateShareAsync(Guid shareId, ShareEventDto request)
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

        public async Task<EventShare> GetShareByIdAsync(Guid shareId)
        {
            var share = await _shareRepository.GetByIdAsync(shareId); // Utilize o método correto
            return share; // Retorna o compartilhamento ou null
        }
    }
}