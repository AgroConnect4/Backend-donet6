using agroApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
public interface IRoleRepository
    {
        Task<Role> GetByNameAsync(string roleName);
        Task<Role> AddAsync(Role role);
    }
}
    