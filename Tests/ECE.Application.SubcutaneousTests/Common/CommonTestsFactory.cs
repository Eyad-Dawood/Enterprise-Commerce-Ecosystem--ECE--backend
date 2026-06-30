using Microsoft.EntityFrameworkCore;
using ECE.Infrastructure.Data;

namespace ECE.Application.SubcutaneousTests.Common;

public static class CommonTestsFactory
{
    static public AppDbContext CreateDbContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options, new NoOpMediator());
    }
}
