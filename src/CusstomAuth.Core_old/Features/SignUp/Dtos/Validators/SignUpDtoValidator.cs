using FluentValidation;

namespace CusstomAuth.Core.Features.SignUp.Dtos.Validators;

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(s => s.FirstName).NotEmpty().WithMessage("Can't be empty");
        RuleFor(s => s.Login).NotEmpty().WithMessage("Can't be empty")
            .EmailAddress().WithMessage("Login should be an email address");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Can't be empty");
    }
}
