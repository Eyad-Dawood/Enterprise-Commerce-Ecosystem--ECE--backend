
namespace ECE.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager,
    ILogger<IdentityService> logger) : IIdentityService
{
    public async Task<Result<AppUserDto>> AuthenticateAsync(string email, string password, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            return InfrastructureIdentityErrors.InvalidLoginAttempt;

        //User Cannot Access His Email Without Confirming , 
        //He Can Access Confirmation Endpoint To Resend Confirmation Email But He Cannot Access Any Other Endpoint Before Confirming His Email

        if (!user.EmailConfirmed)
            return InfrastructureIdentityErrors.EmailNotConfirmed;

        if (!await userManager.CheckPasswordAsync(user, password))
            return InfrastructureIdentityErrors.InvalidLoginAttempt;


        return new AppUserDto(
            user.UserName ?? "Unknown",
            user.Id,
            user.Email!,
            await userManager.GetRolesAsync(user),
            await userManager.GetClaimsAsync(user),
            user.EmailConfirmed);
    }
    public async Task<Result<Deleted>> DeleteUserByIdAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Deleted;

        var deleteResult = await userManager.DeleteAsync(user);

        if (!deleteResult.Succeeded)
        {
            logger.LogError(
                "Failed to delete user with ID {UserId}. Errors: {Errors}",
                userId,
                string.Join(" | ", deleteResult.Errors.Select(e => e.Description)));

            return InfrastructureIdentityErrors.UserDeletionFailed;
        }
        return Result.Deleted;
    }
    public async Task<Result<AppUserDto>> GetUserByIdAsync(string id, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return InfrastructureIdentityErrors.UserNotFoundById;

        return new AppUserDto(
            user.UserName ?? "Unknown",
            user.Id,
            user.Email!,
            await userManager.GetRolesAsync(user),
            await userManager.GetClaimsAsync(user),
            user.EmailConfirmed);
    }
    public async Task<string?> GetUserNameAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);

        return user?.UserName;
    }
    public async Task<bool> UserEmailExists(string email, CancellationToken ct = default) =>
         await userManager.Users.AnyAsync(u => u.Email == email, ct);
    public async Task<Result<bool>> IsEmailConfirmedAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return InfrastructureIdentityErrors.UserNotFoundById;

        return user.EmailConfirmed;
    }
}

