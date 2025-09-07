using Craft.Application.Operations.Queries.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Validators
{
    public class GetUserPreferencesValidator : AbstractValidator<GetUserPreferencesQuery>
    {
        public GetUserPreferencesValidator()
        {
            RuleFor(x => x.UserID)
                .NotNull().WithMessage("UserID cannot be null.")
                .Must(userId => userId != Guid.Empty).WithMessage("UserID must be a valid GUID.");
        }
    }
}
