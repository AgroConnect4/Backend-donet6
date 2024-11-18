using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface IEventShareService
    {
        Task<Guid> ShareEventAsync(ShareEventDto request);
        Task<List<EventShare>> GetSharesByEventIdAsync(Guid eventId);
        Task<EventShare> UpdateShareAsync(Guid shareId, ShareEventDto request);
        Task DeleteShareAsync(Guid shareId);
        Task<EventShare> GetShareByIdAsync(Guid shareId); 
    }
}