namespace ECE.Infrastructure.Identity;

static public class InfrastructureIdentityErrors
{
    private const string ClassName = "User";

    static public readonly Error UserNotFoundByEmail = ApplicationCommonErrors.NotFoundClass(ClassName, "email", "Email");
    static public readonly Error UserNotFoundById = ApplicationCommonErrors.NotFoundClass(ClassName, "id", "Id");
    static public readonly Error EmailNotConfirmed = DomainCommonErrors.CustomConflict(ClassName, "user_email_not_confirmed", "This user hasnt confirm his email yet.");
    static public readonly Error EmailAlreadyConfirmed = DomainCommonErrors.CustomConflict(ClassName, "user_email_already_confirmed", "This user has confirmed his email already.");
    static public readonly Error InvalidLoginAttempt = DomainCommonErrors.CustomUnAuthorized(ClassName, "user_failed_login_attempt", "Email or Password are incorrect.");
    static public readonly Error UserCreationFailed = DomainCommonErrors.CustomFailure(ClassName, "user_creation_failed", "Failed To Create User");
    static public readonly Error UserDeletionFailed = DomainCommonErrors.CustomFailure(ClassName, "user_deletion_failed", "Failed to Delete User");
    static public readonly Error InvalidEmailConfirmationToken = DomainCommonErrors.CustomValidation(ClassName, "invalid_email_confirmation_token", "Email confirmation token is not valid");
    static public readonly Error UserNotAuthenticated = DomainCommonErrors.CustomUnAuthorized(ClassName, "user_not_authenticated", "User is not authenticated.");
}
