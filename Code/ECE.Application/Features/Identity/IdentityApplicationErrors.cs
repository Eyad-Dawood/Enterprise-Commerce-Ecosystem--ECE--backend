namespace ECE.Application.Features.Identity;

static public class IdentityApplicationErrors
{
    private const string ClassName = "User";

    static public readonly Error IdRequired = DomainCommonErrors.RequiredProp(ClassName, "user_id", "User Id");
    static public readonly Error EmailRequired = DomainCommonErrors.RequiredProp(ClassName, "email", "Email");
    static public readonly Error PasswordRequired = DomainCommonErrors.RequiredProp(ClassName, "password", "Password");
    static public readonly Error InvalidEmail = DomainCommonErrors.InvalidProp(ClassName, "email", "Email");
    static public readonly Error InvalidPassword = DomainCommonErrors.InvalidProp(ClassName, "password", "Password");
    static public readonly Error ExpiredAccessTokenInvalid = DomainCommonErrors.InvalidProp(ClassName, "access_token", "Access Token");
    static public readonly Error UserIdClaimInvalid = DomainCommonErrors.InvalidProp(ClassName, "user_id_claim", "User Id Claim");
    static public readonly Error RefreshTokenExpired = DomainCommonErrors.CustomUnAuthorized(ClassName, "refresh_token_expired", "This refresh token has expired");
    static public readonly Error EmailConfirmationTokenRequired = DomainCommonErrors.RequiredProp(ClassName, "email_confirmation_refresh_token", "Email Confirmation Refresh Token");
    static public readonly Error PasswordResetTokenRequired = DomainCommonErrors.RequiredProp(ClassName, "password_reset_token", "Password Reset Token");
}
