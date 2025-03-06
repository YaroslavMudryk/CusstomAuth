namespace CusstomAuth.Core.Stores;

public interface IUserStore : IStore<IdentityUser>
{
    Task<bool> IsExistUserByLoginAsync(string login);
    Task<IdentityUser> GetUserByLoginAsync(string login);
}
