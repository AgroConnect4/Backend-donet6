using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface IEventCommentService
    {
        Task<Guid> CreateCommentAsync(CreateEventCommentDto request, Guid userId);
        Task<List<EventComment>> GetCommentsByEventIdAsync(Guid eventId);
        Task<EventComment> UpdateCommentAsync(Guid commentId, UpdateEventCommentDto request);
        Task DeleteCommentAsync(Guid commentId);
        Task<EventComment> GetCommentByIdAsync(Guid commentId); 
    }
}