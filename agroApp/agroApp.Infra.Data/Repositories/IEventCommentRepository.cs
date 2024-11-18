using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IEventCommentRepository
    {
        Task<EventComment> GetByIdAsync(Guid commentId);
        Task<List<EventComment>> GetAllAsync();
        Task<EventComment> AddAsync(EventComment comment);
        Task<EventComment> UpdateAsync(EventComment comment);
        Task DeleteAsync(Guid commentId);
        Task<EventComment> GetCommentByIdAsync(Guid commentId);
        Task<List<EventComment>> GetCommentsByEventIdAsync(Guid eventId); 
    }
}