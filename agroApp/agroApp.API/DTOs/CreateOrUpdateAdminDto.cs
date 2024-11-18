using agroApp.Domain.Entities;

namespace agroApp.API.DTOs
{
    public class CreateOrUpdateAdminDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CurrentPassword { get; set; } // Para atualizar a senha
        public bool AddAdminRole { get; set; }     // Para adicionar a role de administrador
        public bool RemoveAdminRole { get; set; }  // Para remover a role de administrador
        public bool AddUserRole { get; set; }      // Para adicionar a role de usuário
        public bool RemoveUserRole { get; set; }   // Para remover a role de usuário
    }
}