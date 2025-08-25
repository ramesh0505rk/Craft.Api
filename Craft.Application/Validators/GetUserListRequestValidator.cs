using Craft.Application.Operations.Queries.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Validators
{
    public class GetUserListRequestValidator : AbstractValidator<GetUserListQuery>
    {
        public GetUserListRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.")
                .NotEmpty().WithMessage("Page number cannot be empty.")
                .NotNull().WithMessage("Page number cannot be null.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .NotEmpty().WithMessage("PageSize cannot be empty.")
                .NotNull().WithMessage("PageSize cannot be null.");

            RuleFor(x => x.SearchString)
                .MaximumLength(100).WithMessage("SearchString must be less than or equal to 100 characters.");
        }
    }
}
