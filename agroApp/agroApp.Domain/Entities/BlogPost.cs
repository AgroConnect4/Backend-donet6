using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace agroApp.Domain.Entities
{
    public class BlogPost
    {
        public Guid id { get; set; }
        private string _title;
        public string Title
        {
            get { return _title; }
            set 
            {
                ValidateTitle(value);
                _title = value; 
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set 
            {
                ValidateContent(value);
                _content = value; 
            }
        }
        public DateTime DateCreated { get; set; }

        public Guid UserId { get; set; } // Autor da postagem
        public User User { get; set; }  // Referência ao autor

        private void ValidateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title não pode ser nulo ou vazio.");
            }
        }

        // Validação para Content
        private void ValidateContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Content não pode ser nulo ou vazio.");
            }
        }

        private void ValidateBio(string bio)
        {
            if (string.IsNullOrEmpty(bio))
            {
                throw new ArgumentNullException("Bio não pode ser nulo ou vazio.");
            }
        }

        private void ValidateProfilePicture(string profilePicture)
        {
            if (string.IsNullOrEmpty(profilePicture))
            {
                throw new ArgumentException("ProfilePicture não pode ser nulo ou vazio.");
            }

            if (!Uri.TryCreate(profilePicture, UriKind.Absolute, out var uriResult) || uriResult == null)
            {
                throw new FormatException("ProfilePicture deve ser uma URL válida.");
            }
        }

        private void ValidateCoverPicture(string coverPicture)
        {
            if (string.IsNullOrEmpty(coverPicture))
            {
                throw new ArgumentException("CoverPicture não pode ser nulo ou vazio.");
            }
            // Adicionar validação de URL aqui se necessário
        }

        private void ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Description não pode ser nulo ou vazio.");
            }
        }

        // Validação para URLs (opcional)
        private void ValidateUrl(string url)
        {
            if (!string.IsNullOrEmpty(url)) // Se o URL não for nulo ou vazio
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) || uriResult == null)
                {
                    throw new FormatException("URL inválida.");
                }
            }
        }

        private void ValidateWhatsAppNumber(string whatsappNumber)
        {
            if (!string.IsNullOrEmpty(whatsappNumber)) // Se o número não for nulo ou vazio
            {
                // Adicione a lógica de validação do número do WhatsApp aqui. 
                // Você pode usar Regex para verificar o formato.
                // Exemplo (simplificado):
                if (!Regex.IsMatch(whatsappNumber, @"^\+\d{10,15}$"))
                {
                    throw new FormatException("Número do WhatsApp inválido.");
                }
            }
        }
    }

}
