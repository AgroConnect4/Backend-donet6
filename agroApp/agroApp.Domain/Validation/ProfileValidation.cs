/*using FluentValidation;
using agroApp.Domain.Entities;

namespace agroApp.Domain.Validation
{
    public class ProfileValidator : AbstractValidator<Profile>
    {
        public ProfileValidator()
        {
            RuleFor(p => p.ProfilePicture)
                .NotEmpty()
                .WithMessage("Foto de perfil é obrigatória.");
            
            RuleFor(p => p.CoverPicture)
                .NotEmpty()
                .WithMessage("Capa é obrigatória.");
            
            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Descrição é obrigatória.");
        }
    }
}*/