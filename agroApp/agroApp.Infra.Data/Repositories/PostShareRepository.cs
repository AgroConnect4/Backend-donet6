using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public class PostShareRepository : IPostShareRepository
    {
        private readonly AppDbContext _context;

        public PostShareRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PostShare> GetByIdAsync(Guid shareId)
        {
            return await _context.PostShares.FindAsync(shareId);
        }

        public async Task<List<PostShare>> GetAllAsync()
        {
            return await _context.PostShares.ToListAsync();
        }

        public async Task<PostShare> AddAsync(PostShare share)
        {
            _context.PostShares.Add(share);
            await _context.SaveChangesAsync();
            return share;
        }

        public async Task<PostShare> UpdateAsync(PostShare share)
        {
            _context.Entry(share).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return share;
        }

        public async Task DeleteAsync(Guid shareId)
        {
            var share = await _context.PostShares.FindAsync(shareId);
            if (share != null)
            {
                _context.PostShares.Remove(share);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PostShare>> GetSharesByPostIdAsync(Guid postId)
        {
            return await _context.PostShares.Where(s => s.PostId == postId).ToListAsync();
        }
    }
}