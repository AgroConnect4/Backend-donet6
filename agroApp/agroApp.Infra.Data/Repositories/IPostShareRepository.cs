using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IPostShareRepository
    {
        Task<PostShare> GetByIdAsync(Guid shareId);
        Task<List<PostShare>> GetSharesByPostIdAsync(Guid postId);
        Task<List<PostShare>> GetAllAsync();
        Task<PostShare> AddAsync(PostShare share);
        Task<PostShare> UpdateAsync(PostShare share);
        Task DeleteAsync(Guid shareId);
    }
}