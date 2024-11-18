using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agroApp.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                ValidateName(value); // Chama o método de validação
                _name = value;
            }
        }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name não pode ser nulo ou vazio.");
            }
        }
    }
}