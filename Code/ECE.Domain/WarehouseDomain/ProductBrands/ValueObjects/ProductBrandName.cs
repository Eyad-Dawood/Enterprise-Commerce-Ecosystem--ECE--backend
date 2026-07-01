
namespace ECE.Domain.WarehouseDomain.ProductBrands.ValueObjects;

public record ProductBrandName : ValueObject<string>
{
    public static readonly Regex ProductBrandNameRegx =
        new(@"^[A-Za-z\u0600-\u06FF0-9 ]*$", RegexOptions.Compiled);

    public const int MinLength = 3;
    public const int MaxLength = 300;

    private ProductBrandName(string Value) : base(Value)
    {
    }

    public static string Normalize(string value)
    {
        return ArabicNormalizer.Normalize(value.Trim().ToLower());
    }
    private static Result<string> Validate(string value)
    {
        if (value.Length > MaxLength || value.Length < MinLength)
            return ProductBrandErrors.InvalidProductBrandName;

        if (!ProductBrandNameRegx.IsMatch(value))
            return ProductBrandErrors.InvalidProductBrandName;

        return value;
    }

    public static Result<ProductBrandName> Create(string? Value)
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ProductBrandErrors.ProductBrandNameRequired;

        var normalized = Normalize(Value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new ProductBrandName(validationResult.Value);

        return validationResult.Errors;
    }
}