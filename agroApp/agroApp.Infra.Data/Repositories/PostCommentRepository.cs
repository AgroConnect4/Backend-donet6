using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public class PostCommentRepository : IPostCommentRepository
    {
        private readonly AppDbContext _context;

        public PostCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PostComment> GetByIdAsync(Guid commentId)
        {
            return await _context.PostComments.FindAsync(commentId) ?? null; // Retorna null se não encontrado
        }

        public async Task<List<PostComment>> GetAllAsync()
        {
            return await _context.PostComments.ToListAsync();
        }

        public async Task<PostComment> AddAsync(PostComment comment)
        {
            _context.PostComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<PostComment> UpdateAsync(PostComment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Guid commentId)
        {
            var comment = await _context.PostComments.FindAsync(commentId);
            if (comment != null)
            {
                _context.PostComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Trate o caso em que o comentário não existe
                // Por exemplo, lance uma exceção ou registre um log
            }
        }

        public async Task<List<PostComment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await _context.PostComments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}