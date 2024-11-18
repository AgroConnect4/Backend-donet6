using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IPostCommentRepository
    {
        Task<PostComment> GetByIdAsync(Guid commentId);
        Task<List<PostComment>> GetAllAsync();
        Task<List<PostComment>> GetCommentsByPostIdAsync(Guid postId);
        Task<PostComment> AddAsync(PostComment comment);
        Task<PostComment> UpdateAsync(PostComment comment);
        Task DeleteAsync(Guid commentId);
    }
}