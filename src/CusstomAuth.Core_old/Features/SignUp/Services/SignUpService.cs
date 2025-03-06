using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Features.SignUp.Dtos;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Services.Email;
using CusstomAuth.Core.Services.Email.Dtos;
using Extensions.Password;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CusstomAuth.Core.Features.SignUp.Services;

public interface ISignUpService
{
    Task<int> SignUpAsync(SignUpDto signUpDto);
}

public class SignUpService(
    AuthDbContext db,
    TimeProvider dateTimeProvider,
    IUserManager userManager,
    IRoleManager roleManager,
    IEmailService emailService,
    IOptions<CusstomAuthOptions> options) : ISignUpService
{
    public async Task<int> SignUpAsync(SignUpDto signUpDto)
    {
        if (!Regex.IsMatch(signUpDto.Password, options.Value.Password.Regex))
            throw new ValidationException([new ValidationFailure("password", options.Value.Password.ErrorRegexMessages["en"])]);

        await using var transaction = await db.BeginTransactionAsync();

        var existUser = await userManager.IsExistUsersAsync(user => user.Login == signUpDto.Login);
        if (existUser)
            throw new UserAlreadyRegisterdException();

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;

        var userPassword = signUpDto.Password.GeneratePasswordHash();

        var newPassword = new AuthPassword
        {
            Hint = signUpDto.PasswordHint,
            PasswordHash = userPassword,
            IsActive = true,
            ActivatedAt = utcNow,
            DeactivatedAt = null
        };

        var newUser = new AuthUser(signUpDto.FirstName, signUpDto.LastName, signUpDto.Login, userPassword)
        {
            Passwords = [newPassword]
        };

        var role = await roleManager.GetDefaultRoleAsync();

        var newUserRole = new AuthUserRole
        {
            User = newUser,
            RoleId = role.Id,
            IsActive = true,
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(10),
        };

        await db.Users.AddAsync(newUser);
        await db.UserRoles.AddAsync(newUserRole);
        await db.SaveChangesAsync();

        if (options.Value.Endpoints[HttpActions.ConfirmAction].IsAvailable)
        {
            var confirmAccount = AuthConfirm.NewWithAccount(utcNow, Generator.GetConfirmCode(options.Value.Codes[CodeConfigs.ConfirmAccount]));
            confirmAccount.UserId = newUser.Id;
            newUser.IsConfirmed = false;
            await db.Confirms.AddAsync(confirmAccount);

            await emailService.SendEmailAsync(new EmailRequestDto
            {
                Subject = "Confirmation account",
                Body = $"This is your code: {confirmAccount.Code} for {confirmAccount.UserId}",
                UserEmail = newUser.Login
            });
        }
        else
        {
            newUser.IsConfirmed = true;
        }

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return newUser.Id;
    }
}
