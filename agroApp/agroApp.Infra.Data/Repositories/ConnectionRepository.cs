using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context; // Assuma que você tem um contexto de banco de dados
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly AppDbContext _context; // Use o seu contexto do EF Core

        public ConnectionRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Connection> GetConnectionAsync(Guid userId, Guid connectedUserId) // Changed parameter type
        {
             return await _context.Connections
                .FirstOrDefaultAsync(c => (c.UserId == userId && c.ConnectedUserId == connectedUserId) ||
                                          (c.UserId == connectedUserId && c.ConnectedUserId == userId));
        }

        public async Task<Connection> AddAsync(Connection connection)
        {
            _context.Connections.Add(connection);
            await _context.SaveChangesAsync();
            return connection;
        }

        public async Task DeleteAsync(Connection connection)
        {
            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Guid>> GetConnectedUserIdsAsync(Guid userId)
        {
            return await _context.Connections
                .Where(c => c.UserId == userId || c.ConnectedUserId == userId)
                .Select(c => c.UserId == userId ? c.ConnectedUserId : c.UserId)
                .Distinct()
                .ToListAsync();
        }
    }
}