namespace ECE.Infrastructure.Common.Errors;

public static class InfrastrucureConcurrencyErrors
{
    private const string ClassName = "Infrastructure";

    static public readonly Error InvalidConcurrencyFormat = DomainCommonErrors.CustomValidation(
        ClassName,
        "invalid_concurrency_format",
        "The provided concurrency token format is invalid.");

    static public readonly Error ConcurrencyTokenRequired = DomainCommonErrors.CustomValidation(
        ClassName,
        "concurrency_token_required",
        "A concurrency token is required for this operation.");
}
