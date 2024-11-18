using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Http; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; // Importar o namespace do UserManager

namespace agroApp.API.Services
{
    public class EventCommentService : IEventCommentService
    {
        private readonly IEventCommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager; // Adicione UserManager

        public EventCommentService(
            IEventCommentRepository commentRepository, 
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager
        ) // Injete o UserManager
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Guid> CreateCommentAsync(CreateEventCommentDto request, Guid userId)
        {
            // No need for user existence check here – the controller already did it!
            var comment = new EventComment
            {
                EventId = request.EventId,
                UserId = userId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);
            return comment.Id;
        }

        public async Task<List<EventComment>> GetCommentsByEventIdAsync(Guid eventId)
        {
            return await _commentRepository.GetCommentsByEventIdAsync(eventId);
        }

        public async Task<EventComment> UpdateCommentAsync(Guid commentId, UpdateEventCommentDto request)
        {
            // Validação do request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Os dados da atualização do comentário não podem ser nulos.");
            }

            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                // Retorna null, em vez de lançar exceção
                return null; 
            }

            // Validação do ID do usuário (se necessário)
            // ...

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow; // Atualiza a data de atualização
            return await _commentRepository.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<EventComment> GetCommentByIdAsync(Guid commentId)
        {
            return await _commentRepository.GetByIdAsync(commentId);
        }
    }
}