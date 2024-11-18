using agroApp.API.DTOs;
using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using agroApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace agroApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Autenticação obrigatória para todos os métodos
    public class PostCommentsController : ControllerBase
    {
        private readonly IPostCommentService _commentService;
        private readonly UserManager<User> _userManager; // Adicione UserManager

        public PostCommentsController(IPostCommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreatePostCommentDto request)
        {
            string userIdString = _userManager.GetUserId(User);
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var commentId = await _commentService.CreateCommentAsync(request, userId);
                return CreatedAtAction(nameof(GetCommentById), new { id = commentId }, null);
            }
            else
            {
                return BadRequest("ID de usuário inválido."); // Retorna um erro apropriado
            }
        }

        [HttpGet("{postId}/comments")]
        public async Task<ActionResult<List<PostComment>>> GetCommentsByPostId(Guid postId)
        {
            var comments = await _commentService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<PostComment>> GetCommentById(Guid commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdatePostCommentDto request)
        {
            string userIdString = _userManager.GetUserId(User);
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("ID de usuário inválido.");
            }

            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null || comment.UserId != userId)
            {
                return Unauthorized();
            }

            var updatedComment = await _commentService.UpdateCommentAsync(commentId, request);
            return Ok(updatedComment);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            string userIdString = _userManager.GetUserId(User);
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("ID de usuário inválido.");
            }

            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null || comment.UserId != userId)
            {
                return Unauthorized();
            }

            await _commentService.DeleteCommentAsync(commentId);
            return NoContent();
        }
    }
}