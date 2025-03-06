using FluentValidation;

namespace CusstomAuth.Core.Features.SignIn.Dtos.Validators;

public class SignInDtoValidator : AbstractValidator<SignInDto>
{
    public SignInDtoValidator()
    {
        RuleFor(s => s.Login).NotEmpty().WithMessage("Can't be blank");
        RuleFor(s => s.Login).EmailAddress().WithMessage("This should be an email address");
        RuleFor(s => s.Login).MinimumLength(5).WithMessage("Should not be less then 5 symbols")
            .MaximumLength(200).WithMessage("Should be less then 200 symbols");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Can't be blank");
        RuleFor(s => s.Lang).NotEmpty().WithMessage("Can't be blank");
    }
}
