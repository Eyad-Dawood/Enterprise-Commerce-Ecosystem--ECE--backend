namespace ECE.Infrastructure.Data;


public class AppDbContextInitialiser(
    ILogger<AppDbContextInitialiser> logger,
    AppDbContext context,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager)
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
        logger.LogInformation("Seeding database...");

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
    public async Task SeedRoles(string adminRoleName)
    {
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            logger.LogInformation("Admin role created");
        }
    }
    private async Task<AppUser?> CreateUserIfNotExistsAsync(AppUser user, string password, string role)
    {
        var existingUser = await userManager.FindByEmailAsync(user.Email!);

        if (existingUser is not null)
        {
            logger.LogInformation("User {Email} already exists", user.Email);
            return existingUser;
        }

        user.Id = Guid.NewGuid().ToString();

        var createResult = await userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create user {Email}: {Errors}",
                user.Email,
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return null;
        }

        var roleResult = await userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            logger.LogError("Failed to assign role {Role} to user {Email}: {Errors}",
                role,
                user.Email,
                string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        logger.LogInformation("User {Email} created with role {Role}", user.Email, role);

        return user;
    }


    #region Flow
    #endregion


    #endregion

    public async Task TrySeedAsync()
    {
        await SeedRoles("Admin");


        var adminUser = new AppUser()
        {
            UserName = "admin",
            Email = "admin@Ece.com",
            EmailConfirmed = true,
        };

        await CreateUserIfNotExistsAsync(adminUser, "Admin123", "Admin");

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
