using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId);
        Task<Notification> AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task<Notification> GetByIdAsync(Guid notificationId); // Adicionar este método
        Task DeleteAsync(Notification notification); // Adicionar este método
    }
}