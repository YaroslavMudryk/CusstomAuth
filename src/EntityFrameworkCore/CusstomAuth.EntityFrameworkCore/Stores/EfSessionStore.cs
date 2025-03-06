using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfSessionStore<DbContext>(DbContext dbContext) : BaseStore<IdentitySession, DbContext>(dbContext), ISessionStore
        where DbContext : IdentityDbContext
{
    public async Task<List<IdentitySession>> GetActiveSessionsAsync(int userId)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .Where(s => s.UserId == userId && s.Status == SessionStatus.Active || s.Status == SessionStatus.Pending)
            .OrderBy(s => s.Status).OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<IdentitySession>> GetSessionsAsync(int userId, string[] sessionIds)
    {
        return await dbContext.Sessions.AsNoTracking().Where(s => sessionIds.Contains(s.PublicId) && s.UserId == userId).ToListAsync();
    }

    public async Task<List<IdentitySession>> GetUnactiveSessionsAsync(int userId)
    {
        return await dbContext.Sessions
            .AsNoTracking().Where(s => s.UserId == userId && s.Status == SessionStatus.Completed || s.Status == SessionStatus.Terminated)
            .OrderByDescending(s => s.DeactivatedAt)
            .ToListAsync();
    }

    public async Task UpdateSessionsAsync(List<IdentitySession> sessions)
    {
        dbContext.Sessions.UpdateRange(sessions);
        await dbContext.SaveChangesAsync();
    }
}
