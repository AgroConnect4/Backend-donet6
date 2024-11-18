using agroApp.API.DTOs;
using agroApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using agroApp.Domain.Entities;
using System.Threading.Tasks;
using agroApp.Infra.Data.Repositories;

namespace agroApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetAllEvents()
        {
            var events = await _eventsService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(Guid id)
        {
            var eventDto = await _eventsService.GetEventByIdAsync(id);
            if (eventDto == null)
            {
                return NotFound();
            }
            return Ok(eventDto);
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            var eventDto = await _eventsService.CreateEventAsync(createEventDto);

            // Verifique se o evento foi criado com sucesso
            if (eventDto == null)
            {
                // Se o evento for nulo, você pode retornar um erro:
                return BadRequest("Erro ao criar o evento.");
            }

            // Caso contrário, o evento foi criado
            return Ok("Evento Cadastrado com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventDto updateEventDto)
        {
            await _eventsService.UpdateEventAsync(updateEventDto, id); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            await _eventsService.DeleteEventAsync(id);
            return NoContent();
        }
    } 
}