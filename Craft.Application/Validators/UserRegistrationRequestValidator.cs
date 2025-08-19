using Craft.Application.Operations.Commands.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationRequestValidator()
        {
        }
    }
}
