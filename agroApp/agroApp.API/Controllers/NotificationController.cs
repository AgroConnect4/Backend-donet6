using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using agroApp.Infra.Data.Repositories;
using System.Security.Claims;

namespace agroApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // Obter todas as notificações do usuário atual
        [HttpGet]
        public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
        {
            // Obter o ID do usuário atual do token JWT.  Usamos ClaimTypes.NameIdentifier que é um padrão comum, mas pode variar dependendo da sua implementação de autenticação.
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //Verificar se o userId foi encontrado
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(); // Retorna um código 401 (Unauthorized) se o ID do usuário não for encontrado.
            }

            //Converter a string para Guid
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("ID de usuário inválido."); // Retorna um código 400 (BadRequest) se a conversão falhar.
            }


            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);

            return Ok(notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead
            }).ToList());
        }

        // Marcar notificação como lida
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(string notificationIdString)
        {
            //Recebe o ID como string e tenta converter para Guid
            if (!Guid.TryParse(notificationIdString, out Guid notificationId))
            {
                return BadRequest("ID de notificação inválido.");
            }

            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return NotFound(); // Retorna um código 404 (NotFound) se a notificação não for encontrada.
            }

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);

            return NoContent(); // Retorna um código 204 (NoContent) indicando sucesso sem conteúdo no corpo da resposta.
        }

        // Excluir notificação
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(string notificationIdString)
        {
             //Recebe o ID como string e tenta converter para Guid
            if (!Guid.TryParse(notificationIdString, out Guid notificationId))
            {
                return BadRequest("ID de notificação inválido.");
            }

            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }

            await _notificationRepository.DeleteAsync(notification);

            return NoContent();
        }
    }
}