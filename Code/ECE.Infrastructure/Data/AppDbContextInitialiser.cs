namespace ECE.Infrastructure.Data;


public class AppDbContextInitialiser(
    ILogger<AppDbContextInitialiser> logger,
    AppDbContext context)
{
    private const int DatabaseStartupMaxAttempts = 12;
    private static readonly TimeSpan DatabaseStartupDelay = TimeSpan.FromSeconds(5);

    public async Task InitialiseAsync()
    {
        for (var attempt = 1; attempt <= DatabaseStartupMaxAttempts; attempt++)
        {
            try
            {
                await context.Database.MigrateAsync();
                return;
            }
            catch (Exception ex) when (IsTransientDatabaseStartupFailure(ex) && attempt < DatabaseStartupMaxAttempts)
            {
                logger.LogWarning(
                    ex,
                    "Database is not ready yet. Retrying database initialization attempt {Attempt} of {MaxAttempts} in {DelaySeconds} seconds.",
                    attempt,
                    DatabaseStartupMaxAttempts,
                    DatabaseStartupDelay.TotalSeconds);

                await Task.Delay(DatabaseStartupDelay);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    #region Methods




    #region Flow
    #endregion


    #endregion

    public Task TrySeedAsync()
    {
        return Task.CompletedTask;
    }
    #region Helpers
    private async Task<bool> DatabaseAlreadyExistsAsync()
    {
        try
        {
            return await context.Database.CanConnectAsync();
        }
        catch (Exception ex) when (GetSqlException(ex)?.Number is 4060)
        {
            return false;
        }
    }

    private static bool IsTransientDatabaseStartupFailure(Exception ex)
    {
        var sqlException = GetSqlException(ex);

        if (sqlException is null)
            return false;

        return sqlException.Number is 4060 or 53 or -2 or 233 or 18456;
    }

    private static Microsoft.Data.SqlClient.SqlException? GetSqlException(Exception ex)
    {
        if (ex is Microsoft.Data.SqlClient.SqlException sqlException)
            return sqlException;

        return ex.InnerException as Microsoft.Data.SqlClient.SqlException;
    }



    #endregion
}

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}
