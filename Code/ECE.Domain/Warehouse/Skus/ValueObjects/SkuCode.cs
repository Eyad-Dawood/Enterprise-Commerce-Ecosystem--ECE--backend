namespace ECE.Domain.Warehouse.Skus.ValueObjects;

public record SkuCode : ValueObject<string>
{
    public static readonly Regex SkuCodeRegex =
      new(@"^[A-Za-z0-9._\-\/]+$", RegexOptions.Compiled);

    public const int MinLength = 1;
    public const int MaxLength = 100;

    private SkuCode(string value) : base(value)
    {
    }

    public static string Normalize(string value)
    {
        return value.Trim().ToUpperInvariant();
    }

    private static Result<string> Validate(string value)
    {
        if (value.Length < MinLength || value.Length > MaxLength)
            return SkuErrors.InvalidSkuCode;

        if (!SkuCodeRegex.IsMatch(value))
            return SkuErrors.InvalidSkuCode;

        return value;
    }

    public static Result<SkuCode> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return SkuErrors.SkuCodeRequired;

        var normalized = Normalize(value);
        var validation = Validate(normalized);

        if (validation.IsFailure)
            return validation.Errors;

        return new SkuCode(validation.Value);
    }
}