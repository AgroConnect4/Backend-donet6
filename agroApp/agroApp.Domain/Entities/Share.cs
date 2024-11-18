using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agroApp.Domain.Entities
{
public class Share
{
    public Guid Id { get; set; }
    public Guid CommentableId { get; set; } // O ID do post ou evento sendo compartilhado
    public Guid CommentableType { get; set; } // "Post" ou "Evento"
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }

    // Você pode ter outras propriedades específicas para compartilhamentos, como uma mensagem de compartilhamento. 

    // Propriedades de navegação (opcional, para relacionamentos):
    public Post Post { get; set; } 
    public Event Evento { get; set; }
}
}