using agroApp.API.DTOs;
using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace agroApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostSharesController : ControllerBase
    {
        private readonly IPostShareService _shareService;

        public PostSharesController(IPostShareService shareService)
        {
            _shareService = shareService;
        }

        [HttpPost]
        public async Task<IActionResult> SharePost([FromBody] SharePostDto request)
        {
            var shareId = await _shareService.SharePostAsync(request);
            return CreatedAtAction(nameof(GetShareById), new { id = shareId.ToString() }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShareById(Guid id)
        {
            var share = await _shareService.GetShareByIdAsync(id); // Utilize o método correto
            if (share == null)
            {
                return NotFound(); // Retorna NotFound se o compartilhamento não for encontrado
            }
            return Ok(share);
        }

        // ... (Outros métodos para obter, atualizar e deletar compartilhamentos)
    }
}