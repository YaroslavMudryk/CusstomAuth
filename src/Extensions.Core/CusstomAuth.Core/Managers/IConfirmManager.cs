using CusstomAuth.Core.Constants;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Stores;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Managers;

public interface IConfirmManager
{
    Task<bool> SendConfirmUserAsync(int userId);
    Task<bool> ConfirmUserAsync(string code, int userId);
}

public class ConfirmManager(
    IUserStore userStore,
    IConfirmStore confirmStore,
    INotifyer notifyer,
    IOptions<IdentityOptions> options,
    TimeProvider timeProvider) : IConfirmManager
{
    public async Task<bool> ConfirmUserAsync(string code, int userId)
    {
        var confirmRequest = await confirmStore.GetUserConfirmByAccountAsync(code, userId);

        if (confirmRequest == null)
            throw new NotFoundException("Code not found");

        if (confirmRequest.IsActivated)
            throw new BadRequestException("Confirmation already activated");

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        if (!confirmRequest.IsActualyRequest(utcNow))
            throw new BadRequestException("Code was expired. Please request new");

        confirmRequest.IsActivated = true;
        confirmRequest.ActivatedAt = utcNow;

        var user = await userStore.GetAsync(userId);
        user.IsConfirmed = true;

        await confirmStore.UpdateAsync(confirmRequest);
        await userStore.UpdateAsync(user);

        await notifyer.ConfirmedUserAsync(userId);

        return true;
    }

    public async Task<bool> SendConfirmUserAsync(int userId)
    {
        var user = await userStore.GetAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (user.IsConfirmed)
            throw new BadRequestException("User account already activated");

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        var newConfirm = IdentityConfirm.NewWithAccount(utcNow, Generator.GetConfirmCode(options.Value.Codes[CodeConfigs.ConfirmAccount]));

        newConfirm.UserId = user.Id;

        await confirmStore.CreateAsync(newConfirm);

        await notifyer.SendConfirmationAsync(newConfirm.Code, userId);

        return true;
    }
}
