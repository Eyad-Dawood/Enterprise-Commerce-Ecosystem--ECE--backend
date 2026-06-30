namespace ECE.Domain.Identity;

public static class RefreshTokenErrors
{
    private const string ClassName = nameof(RefreshToken);
    static public readonly Error IdRequired = DomainCommonErrors.RequiredProp(ClassName, "id", "Id");
    static public readonly Error TokenRequired = DomainCommonErrors.RequiredProp(ClassName, "token", "Token");
    static public readonly Error UserIdRequired = DomainCommonErrors.RequiredProp(ClassName, "user_id", "User Id");
    static public readonly Error InvalidExpirationDate = DomainCommonErrors.InvalidProp(ClassName, "expiration_date", "Expiration Date");
    static public readonly Error AlreadRevoked = DomainCommonErrors.CustomValidation(ClassName, "token_already_revoked", "Token Already Revoked");
    static public readonly Error AlreadyUsed = DomainCommonErrors.CustomValidation(ClassName, "token_already_used", "Token Already Used");
    static public readonly Error CannotUseRevokedToken = DomainCommonErrors.CustomValidation(ClassName, "cannot_use_revoked_token", "Cannot mark a token with [use] while its revoked");

}
