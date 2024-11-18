/*using agroApp.Domain.Entities;
using FluentValidation;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agroApp.Domain.Validation
{
    public class BlogPostValidator : AbstractValidator<BlogPost>
    {
        public BlogPostValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(100).WithMessage("O título pode ter no máximo 100 caracteres.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("O conteúdo é obrigatório.");
        }
    }
}*/