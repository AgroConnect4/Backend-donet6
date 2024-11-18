using agroApp.Domain.Entities;
using System;

namespace agroApp.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // ID do usuário que recebeu a notificação
        public string Message { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        // Propriedade de navegação para o usuário (relação 1:N)
        public User User { get; set; } 
    }
}