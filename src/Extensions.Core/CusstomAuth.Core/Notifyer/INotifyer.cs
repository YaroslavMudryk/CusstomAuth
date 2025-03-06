namespace CusstomAuth.Core.Notifyer;

public interface INotifyer
{
    Task ConfirmedUserAsync(int userId);
    Task SendConfirmationAsync(string code, int userId);

    Task DisableMfaAsync(int userId);
    Task EnabledMfaAsync(int userId);

    Task SessionExtendedForUserAsync(int userId, Guid sessionId);
    Task SessionsClosedAsync(int userId, string[] sessionIds);

    Task UserBlockedAsync(int userId, DateTime blockedUntil);
    Task TryToLoginIntoAccountAsync(int userId);
    Task LoginInPendingAsync(int userId, Guid sessionId);
    Task LoginSuccessfullyAsync(int userId, Guid sessionId);

    Task LogoutAsync(int userId, Guid sessionId);
    Task RegisterAsync(int userId);
}

public class FakeNotifyer : INotifyer
{
    public async Task ConfirmedUserAsync(int userId)
    {
        await Task.CompletedTask;
    }

    public async Task DisableMfaAsync(int userId)
    {
        await Task.CompletedTask;
    }

    public async Task EnabledMfaAsync(int userId)
    {
        await Task.CompletedTask;
    }

    public async Task LoginInPendingAsync(int userId, Guid sessionId)
    {
        await Task.CompletedTask;
    }

    public async Task LoginSuccessfullyAsync(int userId, Guid sessionId)
    {
        await Task.CompletedTask;
    }

    public async Task LogoutAsync(int userId, Guid sessionId)
    {
        await Task.CompletedTask;
    }

    public async Task RegisterAsync(int userId)
    {
        await Task.CompletedTask;
    }

    public async Task SendConfirmationAsync(string code, int userId)
    {
        await Task.CompletedTask;
    }

    public async Task SessionExtendedForUserAsync(int userId, Guid sessionId)
    {
        await Task.CompletedTask;
    }

    public async Task SessionsClosedAsync(int userId, string[] sessionIds)
    {
        await Task.CompletedTask;
    }

    public async Task TryToLoginIntoAccountAsync(int userId)
    {
        await Task.CompletedTask;
    }

    public async Task UserBlockedAsync(int userId, DateTime blockedUntil)
    {
        await Task.CompletedTask;
    }
}
