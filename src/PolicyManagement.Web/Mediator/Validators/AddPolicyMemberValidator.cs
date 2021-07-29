using System.Text.RegularExpressions;
using FluentValidation;
using PolicyManagement.Web.Mediator.Requests;

namespace PolicyManagement.Web.Mediator.Validators
{
    public class AddPolicyMemberValidator : AbstractValidator<AddPolicyMemberRequest>
    {
        public AddPolicyMemberValidator()
        {
            RuleFor(m => m.PolicyNumber)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(20)
                .WithMessage("{PropertyName} cannot exceed {MaxLength} characters");

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .Matches("^[A-Z]{1}[A-Za-z]+$")
                .WithMessage("{PropertyName} must contain alphabetic characters only, with a minimum length of 2");

            RuleFor(p => p.LastName)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .Matches("^[A-Z]{1}[A-Za-z]+$")
                .WithMessage("{PropertyName} must contain alphabetic characters only, with a minimum length of 2");

            RuleFor(p => p.IdNumber)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .Matches(new Regex(@"^[0-9]{2}([\s\-]?)[0-9]{6,7}[$1]?[a-zA-Z]{1}[$1]?[0-9]{2}$"))
                .WithMessage("{PropertyName} must be a valid ID number in the format 00-000000-A-00");

            RuleFor(p => p.DateOfBirth)
                .NotEmpty()
                .LessThan(System.DateTime.Today)
                .WithMessage("{PropertyName} must be a past date");

            RuleFor(p => p.Gender)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .IsEnumName(typeof(GenderType))
                .WithMessage("{PropertyName} must be a valid gender");

        }
    }
}
