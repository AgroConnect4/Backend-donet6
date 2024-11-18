using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using agroApp.Infra.Data.Context;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace agroApp.API.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionRepository _connectionRepository;
        private readonly UserManager<User> _userManager; // Assumindo que você está usando Identity
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context; 
    
        public ConnectionService(AppDbContext context, IUserRepository userRepository, IConnectionRepository connectionRepository, UserManager<User> userManager)
        {
            _context = context;
            _userRepository = userRepository;
            _connectionRepository = connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Connection> CreateConnectionAsync(Guid userId, Guid connectedUserId)
        {
            // Obter os usuários do banco de dados
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var connectedUser = await _userManager.FindByIdAsync(connectedUserId.ToString());

            if (user == null || connectedUser == null)
            {
                throw new ArgumentException("Usuários inválidos para a conexão.");
            }

            // Criar a conexão
            var connection = new Connection { UserId = userId, ConnectedUserId = connectedUserId };

            // Adicionar a conexão aos usuários
            user.Connections.Add(connection);
            connectedUser.Connections.Add(connection);

            // Salvar as alterações no banco de dados
            await _connectionRepository.AddAsync(connection);

            return connection;
        }

        public async Task DeleteConnectionAsync(Guid userId, Guid connectedUserId)
        {
            // Obter a conexão
            var connection = await _connectionRepository.GetConnectionAsync(userId, connectedUserId);

            if (connection == null)
            {
                throw new ArgumentException("Conexão não encontrada.");
            }

            // Remover a conexão dos usuários
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var connectedUser = await _userManager.FindByIdAsync(connectedUserId.ToString());

            user.Connections.Remove(connection);
            connectedUser.Connections.Remove(connection);

            // Remover a conexão do banco de dados
            await _connectionRepository.DeleteAsync(connection);
        }

        public async Task<List<User>> GetConnectedUsersAsync(Guid userId)
        {
            // Busca eficiente usando o DbSet do EF Core e Include para carregar as conexões
            return await _context.Users
                .Include(u => u.Connections) // Inclui as conexões para evitar N+1 queries
                .Where(u => u.Connections.Any(c => c.UserId == userId || c.ConnectedUserId == userId))
                .ToListAsync();
        }
    }
}