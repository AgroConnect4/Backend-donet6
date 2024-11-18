using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using agroApp.Infra.Data.Repositories;

namespace agroApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;

        public UserProfileController(IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        // Obter informações do perfil do usuário
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Converter o usuário para um UserProfileDto
            var userDto = new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                
            };

            // Obter as notificações do usuário
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);

            // Adicionar as notificações ao UserProfileDto
            userDto.Notifications = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead
            }).ToList();

            return Ok(userDto);
        }

        // Marcar notificação como lida
        [HttpPut("notifications/{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);

            return NoContent();
        }

        // Excluir notificação
        [HttpDelete("notifications/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(Guid notificationId)
        {
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