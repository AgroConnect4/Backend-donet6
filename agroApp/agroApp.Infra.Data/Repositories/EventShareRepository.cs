using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public class EventShareRepository : IEventShareRepository
    {
        private readonly AppDbContext _context;

        public EventShareRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EventShare> GetByIdAsync(Guid shareId)
        {
            return await _context.EventShares.FindAsync(shareId);
        }

        public async Task<List<EventShare>> GetAllAsync()
        {
            return await _context.EventShares.ToListAsync();
        }

        public async Task<EventShare> AddAsync(EventShare share)
        {
            _context.EventShares.Add(share);
            await _context.SaveChangesAsync();
            return share;
        }

        public async Task<EventShare> UpdateAsync(EventShare share)
        {
            _context.Entry(share).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return share;
        }

        public async Task DeleteAsync(Guid shareId)
        {
            var share = await _context.EventShares.FindAsync(shareId);
            if (share != null)
            {
                _context.EventShares.Remove(share);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EventShare>> GetSharesByEventIdAsync(Guid eventId)
        {
            return await _context.EventShares
                .Where(s => s.EventId == eventId) // Filtra por EventId
                .ToListAsync();
        }
    }
}