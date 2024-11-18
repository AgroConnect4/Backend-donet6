/*using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using agroApp.Infra.Data.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace agroApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IValidator<BlogPostDTO> _validator;

        public BlogController(IBlogPostRepository blogPostRepository, IValidator<BlogPostDTO> validator)
        {
            _blogPostRepository = blogPostRepository;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var posts = _blogPostRepository.GetAll();
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] BlogPostDTO blogPostDto)
        {
            var validationResult = _validator.Validate(blogPostDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Obtendo o ID do usuário atual e convertendo para int
            var userIdString = GetCurrentUserId(); // Isso retorna uma string
            if (!int.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("Invalid user ID."); // Retorna um erro se a conversão falhar
            }

            var blogPost = new BlogPost
            {
                Title = blogPostDto.Title,
                Content = blogPostDto.Content,
                DateCreated = DateTime.Now,
                UserId = userId // Atribui o valor convertido
            };

            _blogPostRepository.Add(blogPost);
            return CreatedAtAction(nameof(GetAllPosts), new { id = blogPost.Id }, blogPost);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePost(int id)
        {
            var post = _blogPostRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            _blogPostRepository.Remove(post);
            return NoContent();
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retorna o ID do usuário autenticado
        }
    }
}
*/
