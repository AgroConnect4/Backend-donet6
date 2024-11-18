using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using agroApp.API.DTOs;
using Microsoft.Extensions.Logging;

namespace agroApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IPostCommentService _commentService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postService, IPostCommentService commentService, ILogger<PostsController> logger) // Injete IPostCommentService
        {
            _postService = postService;
            _commentService = commentService;
            _logger = logger;   
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto request)
        {
            try
            {
                var post = await _postService.CreatePostAsync(request); // Get the whole Post object
                if (post == null) return StatusCode(500, "Failed to create Post"); //Handle null cases

                return Ok(new[] { post }); // Return the post in a single-element array
            }
            catch (Exception ex)
            {
                // Log the exception properly
                _logger.LogError(ex, "Error creating post: {Message}", ex.Message);
                return StatusCode(500, "An unexpected error occurred while creating the post."); //More generic error message
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllPostsByUserId(Guid userId)
        {
            var posts = await _postService.GetAllPostsByUserIdAsync(userId);
            return Ok(posts);
        }

        [HttpGet("category/{categoryName}")] // Note que o parâmetro agora é categoryName (string)
        public async Task<IActionResult> GetAllPostsByCategoryName(string categoryName)
        {
            var posts = await _postService.GetAllPostsByCategoryNameAsync(categoryName);
            return Ok(posts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto request)
        {
            var post = await _postService.UpdatePostAsync(id, request);
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            await _postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}