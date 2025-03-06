using FluentValidation;

namespace CusstomAuth.Core.Features.SignIn.Dtos.Validators;

public class SignInMfaDtoValidator : AbstractValidator<SignInMfaDto>
{
    public SignInMfaDtoValidator()
    {
        RuleFor(s => s.Code).NotEmpty().WithMessage("Can't be blank");
        RuleFor(s => s.MfaHashKey).NotEmpty().WithMessage("Can't be blank");
    }
}