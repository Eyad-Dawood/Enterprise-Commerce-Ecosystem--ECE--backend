namespace ECE.Domain.Warehouse.Skus.SkuAttributesDefinitions.ValueObjects;

public record SkuAttributeDefinitionName : ValueObject<string>
{

    public static readonly Regex SkuAttributeDefinitionNameRegx =
        new(@"^[A-Za-z0-9 ]*$", RegexOptions.Compiled);

    public const int MinLength = 3;
    public const int MaxLength = 100;

    private SkuAttributeDefinitionName(string Value) : base(Value)
    {
    }

    public static string Normalize(string value)
    {
        return ArabicNormalizer.Normalize(value
            .Trim().ToLower());
    }
    private static Result<string> Validate(string value)
    {
        if (value.Length > MaxLength || value.Length < MinLength)
            return SkuAttributeDefinitionErrors.InvalidSkuAttributeDefinitionName;

        if (!SkuAttributeDefinitionNameRegx.IsMatch(value))
            return SkuAttributeDefinitionErrors.InvalidSkuAttributeDefinitionName;

        return value;
    }

    public static Result<SkuAttributeDefinitionName> Create(string? Value)
    {
        if (string.IsNullOrWhiteSpace(Value))
            return SkuAttributeDefinitionErrors.SkuAttributeDefinitionNameRequired;

        var normalized = Normalize(Value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new SkuAttributeDefinitionName(validationResult.Value);

        return validationResult.Errors;
    }
}
