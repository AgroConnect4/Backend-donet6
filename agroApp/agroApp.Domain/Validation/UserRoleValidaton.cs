/*using FluentValidation;
using agroApp.Domain.Entities;

namespace agroApp.Domain.Validation
{
    public class UserRoleValidator : AbstractValidator<UserRole>
    {
        public UserRoleValidator()
        {
            RuleFor(ur => ur.UserId)
                .NotEmpty()
                .WithMessage("ID do usuário é obrigatório.");
            
            RuleFor(ur => ur.RoleId)
                .NotEmpty()
                .WithMessage("ID do papel é obrigatório.");
        }
    }
}*/