using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agroApp.Domain.Entities
{
    public class Connection
    {
        public Guid Id { get; set; }
        private Guid _userId;
        public Guid UserId { get; set; }
        
        public Guid ConnectedUserId { get; set; } 
        
        public User User { get; set; }
        public User ConnectedUser { get; set; }

        /*private void ValidateUserId(Guid userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId não pode ser nulo ou vazio.");
            }
        }

        private void ValidateConnectedUserId(string connectedUserId)
        {
            if (string.IsNullOrEmpty(connectedUserId))
            {
                throw new ArgumentException("ConnectedUserId não pode ser nulo ou vazio.");
            }
        }*/
    }
}