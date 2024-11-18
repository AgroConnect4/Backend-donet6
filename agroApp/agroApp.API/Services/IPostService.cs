using agroApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using agroApp.API.DTOs;

namespace agroApp.API.Services
{
    public interface IPostService
    {
        Task<Guid> CreatePostAsync(CreatePostDto request);
        Task<Post> GetPostByIdAsync(Guid postId);
        Task<List<Post>> GetAllPostsByUserIdAsync(Guid userId);
        Task<Post> UpdatePostAsync(Guid postId, UpdatePostDto request); // DTO para atualização de post
        Task DeletePostAsync(Guid postId);
        Task<List<Post>> GetAllPostsByCategoryNameAsync(string categoryName);
    }
}