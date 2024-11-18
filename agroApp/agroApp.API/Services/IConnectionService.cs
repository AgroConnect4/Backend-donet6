using agroApp.Domain.Entities;
using System.Threading.Tasks;

namespace agroApp.API.Services
{
    public interface IConnectionService
    {
        Task<Connection> CreateConnectionAsync(Guid userId, Guid connectedUserId);
        Task DeleteConnectionAsync(Guid userId, Guid connectedUserId); 
        Task<List<User>> GetConnectedUsersAsync(Guid userId);
    }
}