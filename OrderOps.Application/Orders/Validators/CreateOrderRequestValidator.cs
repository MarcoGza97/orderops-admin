using FluentValidation;
using OrderOps.Application.Orders.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Orders.Validators
{
    public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0);

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one product is required.");

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.ProductId).GreaterThan(0);
                    item.RuleFor(x => x.Quantity).GreaterThan(0);
                });
        }
    }
}
