using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetByIdAsync(Guid eventId);
        Task<List<Event>> GetAllAsync();
        Task<Event> AddAsync(Event eventItem);
        Task<Event> UpdateAsync(Event eventItem);
        Task DeleteAsync(Guid eventId);
        Task<Event> GetEventByIdAsync(Guid eventId);
        Task<List<Event>> GetAllEventsAsync(); 
        Task<Event> AddEventAsync(Event @event);
        Task<Event> UpdateEventAsync(Event @event);
        Task DeleteEventAsync(Guid eventId);
        Task<List<EventComment>> GetCommentsByEventIdAsync(Guid eventId);
        Task<List<EventShare>> GetSharesByEventIdAsync(Guid eventId);
    }
}