using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task SendEventNotificationAsync(User connectedUser, Event @event)
        {
            var notification = new Notification
            {
                UserId = connectedUser.Id,
                Message = $"O usuário {@event.User?.UserName ?? @event.User?.Email ?? "desconhecido"} criou um novo evento."
            };
            await _notificationRepository.AddAsync(notification);
        }

        public async Task SendPostNotificationAsync(User connectedUser, Post post)
        {
            var notification = new Notification
            {
                UserId = connectedUser.Id,
                Message = $"O usuário {post.User?.UserName ?? post.User?.Email ?? "desconhecido"} criou um novo post."
            };
            await _notificationRepository.AddAsync(notification);
        }
    }
}