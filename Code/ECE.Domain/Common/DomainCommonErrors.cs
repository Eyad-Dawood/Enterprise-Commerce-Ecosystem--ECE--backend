namespace ECE.Domain.Common;

static public class DomainCommonErrors
{
    static public Error RequiredProp(string Class, string PropertyCode, string PropertyDescription) =>
        Error.Validation($"{Class}.{PropertyCode}.required", $"{Class} {PropertyDescription} is required.");

    static public Error InvalidProp(string Class, string PropertyCode, string PropertyDescription, string Details = "") =>
        Error.Validation($"{Class}.{PropertyCode}.invalid", $"{Class} {PropertyDescription} is invalid. {Details}");

    static public Error MaxLengthProp(string Class, string PropertyCode, string PropertyDescription, int maxLength) =>
        Error.Validation($"{Class}.{PropertyCode}.maxlength", $"{Class} {PropertyDescription} must not exceed {maxLength} characters.");

    static public Error CustomValidation(string Class, string Code, string Description) =>
        Error.Validation($"{Class}.{Code}", Description);

    static public Error CustomConflict(string Class, string Code, string Description) =>
        Error.Conflict($"{Class}.{Code}", Description);

    static public Error CustomFailure(string Class, string Code, string Description)
        => Error.Failure($"{Class}.{Code}", Description);

    static public Error CustomUnAuthorized(string Class, string Code, string Description)
       => Error.Unauthorized($"{Class}.{Code}", Description);

    static public Error DateCannotBeInFuture(string Class, string PropertyCode, string PropertyDescription) =>
        Error.Validation($"{Class}.{PropertyCode}.date_cannot_be_in_future", $"{Class} {PropertyDescription} cannot be in the future.");

    static public Error DateCannotBeInPast(string Class, string PropertyCode, string PropertyDescription) =>
        Error.Validation($"{Class}.{PropertyCode}.date_cannot_be_in_past", $"{Class} {PropertyDescription} cannot be in the past.");


    static public Error DateShouldBeInFuture(string Class, string PropertyCode, string PropertyDescription) =>
        Error.Validation($"{Class}.{PropertyCode}.date_should_be_in_future", $"{Class} {PropertyDescription} should be in the future.");

    static public Error DateShouldBeInPast(string Class, string PropertyCode, string PropertyDescription) =>
        Error.Validation($"{Class}.{PropertyCode}.date_should_be_in_past", $"{Class} {PropertyDescription} should be in the past.");

    static private Error InvalidStateTransition(string Class, string FromState, string ToState) =>
        Error.Conflict($"{Class}.invalid_state_transition", $"Cannot transition from {FromState} to {ToState}");

    static private Error InvalidStateTransitionSameState(string Class, string State) =>
       Error.Conflict($"{Class}.invalid_state_transition", $"State is already {State}");

    static public Error InvalidStateTransition<T>(string ClassName, T FromState, T ToState) where T : struct, Enum
    {
        if (FromState.Equals(ToState))
        {
            return InvalidStateTransitionSameState(ClassName, FromState.ToString());
        }

        return InvalidStateTransition(ClassName, FromState.ToString(), ToState.ToString());
    }
}


