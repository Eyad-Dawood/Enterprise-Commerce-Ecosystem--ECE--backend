

namespace ECE.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


