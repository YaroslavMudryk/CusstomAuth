using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Features.Confirm.Services;

public interface IConfirmUserService
{
    Task<bool> ConfirmUserAsync(string code, int userId);
    Task<bool> SendConfirmAsync(int userId);
}

public class ConfirmUserService(AuthDbContext db, TimeProvider dateTimeProvider, IOptions<CusstomAuthOptions> options) : IConfirmUserService
{
    public async Task<bool> ConfirmUserAsync(string code, int userId)
    {
        await using var transaction = await db.BeginTransactionAsync();

        var confirmRequest = await db.Confirms.AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Code == code && s.UserId == userId && s.Type == ConfirmType.Account);

        if (confirmRequest == null)
            throw new NotFoundException("Code not found");

        if (confirmRequest.IsActivated)
            throw new BadRequestException("Confirmation already activated");

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;

        if (!confirmRequest.IsActualyRequest(utcNow))
            throw new BadRequestException("Code was expired. Please request new");

        confirmRequest.IsActivated = true;
        confirmRequest.ActivatedAt = utcNow;

        var user = confirmRequest.User;
        user.IsConfirmed = true;

        db.Confirms.Update(confirmRequest);
        db.Users.Update(user);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return true;
    }

    public async Task<bool> SendConfirmAsync(int userId)
    {
        await using var transaction = await db.BeginTransactionAsync();

        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (user.IsConfirmed)
            throw new BadRequestException("User account already activated");

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;

        var newConfirm = AuthConfirm.NewWithAccount(utcNow, Generator.GetConfirmCode(options.Value.Codes[CodeConfigs.ConfirmAccount]));

        newConfirm.UserId = user.Id;

        await db.Confirms.AddAsync(newConfirm);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return true;
    }
}
