using CusstomAuth.Core.Data;
using CusstomAuth.Core.Options;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Features.SignIn;

public class SignInRequestValidator(IOptions<IdentityOptions> options) : AbstractValidator<SignInRequest>
{

}
