using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public class EventCommentRepository : IEventCommentRepository
    {
        private readonly AppDbContext _context;

        public EventCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EventComment> GetByIdAsync(Guid commentId)
        {
            return await _context.EventComments.FindAsync(commentId);
        }

        public async Task<List<EventComment>> GetAllAsync()
        {
            return await _context.EventComments.ToListAsync();
        }

        public async Task<EventComment> AddAsync(EventComment comment)
        {
            _context.EventComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<EventComment> UpdateAsync(EventComment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Guid commentId)
        {
            var comment = await _context.EventComments.FindAsync(commentId);
            if (comment != null)
            {
                _context.EventComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EventComment>> GetCommentsByEventIdAsync(Guid eventId)
        {
            return await _context.EventComments
                .Where(c => c.EventId == eventId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<EventComment> GetCommentByIdAsync(Guid commentId)
        {
            return await _context.EventComments.FindAsync(commentId);
        }
        
    }
}