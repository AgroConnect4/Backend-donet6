using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IEventShareRepository
    {
        Task<EventShare> GetByIdAsync(Guid shareId);
        Task<List<EventShare>> GetAllAsync();
        Task<EventShare> AddAsync(EventShare share);
        Task<EventShare> UpdateAsync(EventShare share);
        Task DeleteAsync(Guid shareId);
        Task<List<EventShare>> GetSharesByEventIdAsync(Guid eventId); 
    }
}