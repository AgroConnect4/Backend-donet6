using agroApp.Domain.Entities;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IConnectionRepository
    {
        Task<Connection> GetConnectionAsync(Guid userId, Guid connectedUserId);
        Task<Connection> AddAsync(Connection connection);
        Task DeleteAsync(Connection connection);
        Task<List<Guid>> GetConnectedUserIdsAsync(Guid userId);
    }
}