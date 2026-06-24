using FluentValidation;
using OrderOps.Application.Products.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Products.Validators
{
    public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
        }
    }
}
