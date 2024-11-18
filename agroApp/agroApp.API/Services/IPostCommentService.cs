using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface IPostCommentService
    {
        Task<Guid> CreateCommentAsync(CreatePostCommentDto request, Guid userId); 
        Task<List<PostComment>> GetCommentsByPostIdAsync(Guid postId);
        Task<PostComment> UpdateCommentAsync(Guid commentId, UpdatePostCommentDto request);
        Task DeleteCommentAsync(Guid commentId);
        Task<PostComment> GetCommentByIdAsync(Guid commentId);
    }
}