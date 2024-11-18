/*using FluentValidation;
using agroApp.Domain.Entities;

namespace agroApp.Domain.Validation
{
    public class UserValidation : AbstractValidator<User> // Mantém o nome da classe "UserValidation"
    {
        public UserValidation()
        {
            RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("O nome de usuário é obrigatório.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email inválido.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}*/
