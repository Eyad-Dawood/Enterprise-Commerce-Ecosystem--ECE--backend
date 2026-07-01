namespace ECE.Domain.Warehouse.Skus.ValueObjects;

public record SkuBarcode : ValueObject<string>
{
    public const int MaxLength = 14;

    private static readonly Regex BarcodeRegex =
       new(@"^\d{8}|\d{12}|\d{13}|\d{14}$", RegexOptions.Compiled);

    private SkuBarcode(string value) : base(value)
    {
    }

    public static string Normalize(string value)
    {
        return value.Trim();
    }

    private static Result<string> Validate(string value)
    {
        if (!BarcodeRegex.IsMatch(value))
            return SkuErrors.InvalidBarcode;

        return value;
    }

    public static Result<SkuBarcode> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return SkuErrors.BarcodeRequired;

        var normalized = Normalize(value);
        var validation = Validate(normalized);

        if (validation.IsFailure)
            return validation.Errors;

        return new SkuBarcode(validation.Value);
    }
}