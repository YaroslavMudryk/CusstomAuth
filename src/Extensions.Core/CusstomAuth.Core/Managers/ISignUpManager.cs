using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Data;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.ErrorHandling.Extensions;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Passwords;
using CusstomAuth.Core.Stores;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CusstomAuth.Core.Managers;

public interface ISignUpManager
{
    Task<SignUpResponse> SignUpAsync(SignUpRequest signUpRequest, bool isAdmin = false);
}

public class SignUpManager(
    TimeProvider timeProvider,
    IUserStore userStore,
    IUserRoleStore userRoleStore,
    IRoleStore roleStore,
    IConfirmStore confirmStore,
    IPasswordHasher passwordHasher,
    IValidator<SignUpRequest> validator,
    INotifyer notifyer,
    IOptions<IdentityOptions> options) : ISignUpManager
{
    public async Task<SignUpResponse> SignUpAsync(SignUpRequest signUpRequest, bool isAdmin = false)
    {
        await ValidateAndThrowAsync(validator, signUpRequest);

        var existUser = await userStore.IsExistUserByLoginAsync(signUpRequest.Login);
        if (existUser)
            throw new UserAlreadyRegisterdException();

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        var userPassword = passwordHasher.HashPassword(signUpRequest.Password);

        var newPassword = new IdentityPassword
        {
            Hint = signUpRequest.PasswordHint,
            PasswordHash = userPassword,
            IsActive = true,
            ActivatedAt = utcNow,
            DeactivatedAt = null
        };

        var newUser = new IdentityUser(signUpRequest.FirstName, signUpRequest.LastName, signUpRequest.Login, userPassword)
        {
            Passwords = [newPassword]
        };

        if (isAdmin)
        {
            newUser.CanBeBanned = false;
            newUser.CanBeBlocked = false;
        }

        var role = await roleStore.GetDefaultRoleAsync();

        var newUserRole = new IdentityUserRole
        {
            User = newUser,
            RoleId = role.Id,
            IsActive = true,
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(10),
        };

        await userStore.CreateAsync(newUser);
        await userRoleStore.CreateAsync(newUserRole);

        if (!isAdmin && options.Value.Endpoints[HttpActions.ConfirmAction].IsAvailable)
        {
            var confirmAccount = IdentityConfirm.NewWithAccount(utcNow, Generator.GetConfirmCode(options.Value.Codes[CodeConfigs.ConfirmAccount]));
            confirmAccount.UserId = newUser.Id;
            newUser.IsConfirmed = false;
            await confirmStore.CreateAsync(confirmAccount);
        }
        else
        {
            newUser.IsConfirmed = true;
        }

        await userStore.UpdateAsync(newUser);

        await notifyer.RegisterAsync(newUser.Id);

        return new SignUpResponse
        {
            UserId = newUser.Id,
            Login = newUser.Login,
        };
    }

    private async Task ValidateAndThrowAsync(IValidator<SignUpRequest> validator, SignUpRequest signUpRequest)
    {
        if (!Regex.IsMatch(signUpRequest.Password, options.Value.Password.Regex))
            throw new ValidationException([new ValidationFailure("password", options.Value.Password.ErrorRegexMessages["en"])]);

        var result = await validator.ValidateAsync(signUpRequest);

        if (!result.IsValid)
            throw new FailedValidationException(result.MapToFailedValidation());
    }
}
