using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(Guid postId);
        Task<List<Post>> GetAllAsync();
        Task<Post> AddAsync(Post post);
        Task<Post> UpdateAsync(Post post);
        Task DeleteAsync(Guid postId);
        Task<List<Post>> GetAllByUserIdAsync(Guid userId); 
        Task<List<PostComment>> GetCommentsByPostIdAsync(Guid postId);
        Task<List<PostShare>> GetSharesByPostIdAsync(Guid postId);
        Task<List<Post>> GetAllPostsByCategoryNameAsync(string categoryName);
    }
}