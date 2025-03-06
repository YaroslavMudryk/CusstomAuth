using CusstomAuth.Core.Data;
using CusstomAuth.Core.Options;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Features.SignUp;

public class SignUpRequestValidator(IOptions<IdentityOptions> options) : AbstractValidator<SignUpRequest>
{

}
