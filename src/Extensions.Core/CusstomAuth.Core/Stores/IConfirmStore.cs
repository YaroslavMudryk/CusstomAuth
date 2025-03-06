namespace CusstomAuth.Core.Stores;

public interface IConfirmStore : IStore<IdentityConfirm>
{
    Task<IdentityConfirm> GetUserConfirmByAccountAsync(string code, int userId);
}
