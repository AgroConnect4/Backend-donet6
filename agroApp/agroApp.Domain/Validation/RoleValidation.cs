/*using FluentValidation;
using agroApp.Domain.Entities;

namespace agroApp.Domain.Validation
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Nome do papel é obrigatório.");
        }
    }
}*/