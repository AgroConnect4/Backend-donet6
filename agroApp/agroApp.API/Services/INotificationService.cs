using agroApp.Domain.Entities;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface INotificationService
    {
        Task SendEventNotificationAsync(User user, Event @event);
        Task SendPostNotificationAsync(User user, Post post);
    }
}