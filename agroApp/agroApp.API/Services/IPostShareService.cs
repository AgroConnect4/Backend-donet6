using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface IPostShareService
    {
        Task<Guid> SharePostAsync(SharePostDto request);
        Task<List<PostShare>> GetSharesByPostIdAsync(Guid postId);
        Task<PostShare> UpdateShareAsync(Guid shareId, SharePostDto request);
        Task DeleteShareAsync(Guid shareId);
        Task<PostShare> GetShareByIdAsync(Guid shareId);
    }
}