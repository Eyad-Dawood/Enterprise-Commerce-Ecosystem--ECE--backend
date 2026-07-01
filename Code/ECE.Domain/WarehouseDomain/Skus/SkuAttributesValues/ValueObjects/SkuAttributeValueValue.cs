namespace ECE.Domain.WarehouseDomain.Skus.SkuAttributesValues.ValueObjects;

public sealed record SkuAttributeValueValue : ValueObject<string>
{
    public static readonly Regex SkuAttributeValueRegex =
        new(@"^[A-Za-z0-9\u0600-\u06FF .,_+\-()/]*$", RegexOptions.Compiled);

    public const int MinLength = 1;
    public const int MaxLength = 200;

    private SkuAttributeValueValue(string value) : base(value)
    {
    }

    public static string Normalize(string value)
    {
        return ArabicNormalizer.Normalize(value.Trim());
    }

    private static Result<string> Validate(string value)
    {
        if (value.Length < MinLength || value.Length > MaxLength)
            return SkuAttributeValueErrors.InvalidSkuAttributeValue;

        if (!SkuAttributeValueRegex.IsMatch(value))
            return SkuAttributeValueErrors.InvalidSkuAttributeValue;

        return value;
    }

    public static Result<SkuAttributeValueValue> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return SkuAttributeValueErrors.SkuAttributeValueRequired;

        var normalized = Normalize(value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new SkuAttributeValueValue(validationResult.Value);

        return validationResult.Errors;
    }
}