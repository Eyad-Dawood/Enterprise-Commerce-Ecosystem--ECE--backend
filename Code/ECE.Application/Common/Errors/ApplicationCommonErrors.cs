namespace ECE.Application.Common.Errors;

static public class ApplicationCommonErrors
{
    static public Error NotFoundClass(string Class, string PropertyCode, string PropertyDescription) =>
        Error.NotFound($"{Class}.with_{PropertyCode}.not_found", $"{Class} with the specified {PropertyDescription} was not found.");
    static public Error AlreadyExists(string Class, string PropertyCode, string PropertyDescription) =>
       Error.Validation($"{Class}.{PropertyCode}.already_exists", $"A {Class} with the {PropertyDescription} is already exists.");
    static public Error NotFoundProp(string Class, string PropertyCode, string PropertyDescription) =>
        Error.NotFound($"{Class}.{PropertyCode}.not_found", $"{PropertyDescription} was not found in the system.");
}
