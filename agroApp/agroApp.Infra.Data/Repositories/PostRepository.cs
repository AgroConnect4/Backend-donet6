using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Post> GetByIdAsync(Guid postId)
        {
            return await _context.Posts.FindAsync(postId);
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> AddAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeleteAsync(Guid postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<List<PostComment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await _context.PostComments.Where(c => c.PostId == postId).ToListAsync();
        }

        public async Task<List<PostShare>> GetSharesByPostIdAsync(Guid postId)
        {
            return await _context.PostShares.Where(s => s.PostId == postId).ToListAsync();
        }

        public async Task<List<Post>> GetAllPostsByCategoryNameAsync(string categoryName)
        {
            // A condição Where verifica se a lista de categorias do post contém o nome da categoria.
            return await _context.Posts
                .Where(p => p.Categories.Contains(categoryName, StringComparer.OrdinalIgnoreCase)) // Ignora maiúsculas e minúsculas
                .ToListAsync();
        }
    }
}