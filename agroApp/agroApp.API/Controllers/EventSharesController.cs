using agroApp.API.DTOs;
using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using agroApp.Domain.Entities;

namespace agroApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventSharesController : ControllerBase
    {
        private readonly IEventShareService _shareService;

        public EventSharesController(IEventShareService shareService)
        {
            _shareService = shareService;
        }

        [HttpPost]
        public async Task<IActionResult> ShareEvent([FromBody] ShareEventDto request)
        {
            var shareId = await _shareService.ShareEventAsync(request);
            return CreatedAtAction(nameof(GetShareById), new { id = shareId }, null);
        }

        [HttpGet("{shareId}")]
        public async Task<ActionResult<EventShare>> GetShareById(Guid shareId)
        {
            var share = await _shareService.GetShareByIdAsync(shareId); 
            if (share == null)
            {
                return NotFound(); // Retorna NotFound() se o compartilhamento não for encontrado
            }
            return Ok(share);
        }

        // ... (Outros métodos para obter, atualizar e deletar compartilhamentos)
    }
}