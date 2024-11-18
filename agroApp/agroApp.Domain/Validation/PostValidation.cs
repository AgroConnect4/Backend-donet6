/*using FluentValidation;
using agroApp.Domain.Entities;
//using agroApp.API.Services;
using System.Threading;
using System.Threading.Tasks;

namespace agroApp.Domain.Validation
{
    public class PostValidator : AbstractValidator<Post>
    {
        private readonly IPostTypeProvider _postTypeProvider;

        public PostValidator(IPostTypeProvider postTypeProvider)
        {
            _postTypeProvider = postTypeProvider;

            RuleFor(p => p.Content)
                .NotEmpty()
                .WithMessage("Conteúdo do post é obrigatório.");

            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("Título do post é obrigatório.");

            // Em PostValidator
            RuleFor(p => p.PostTypeId)
                .NotEmpty()
                .WithMessage("Tipo de post é obrigatório.")
                .MustAsync(async (postTypeId, cancellation) => await _postTypeProvider.IsValidPostTypeAsync(postTypeId))
                .WithMessage("Tipo de post inválido.");
        }

    }
}*/