namespace ECE.Infrastructure.Common.Errors;

static public class InfrastructureEmailErrors
{
    private const string ClaseName = "Infrastructure";

    static public readonly Error EmailServiceError = DomainCommonErrors.CustomFailure(
        ClaseName,
        "email_service_error",
        "An error occurred while sending the email. Please try again later.");
}
