namespace ECE.Domain.WarehouseDomain.Products.ProductTags.ValueObjects;

public record ProductTagName : ValueObject<string>
{
    public const int MinLength = 2;
    public const int MaxLength = 100;

    private ProductTagName(string Value) : base(Value)
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
            return ProductTagErrors.InvalidProductTagName;

        return value;
    }

    public static Result<ProductTagName> Create(string? Value)
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ProductTagErrors.ProductTagNameRequired;

        var normalized = Normalize(Value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new ProductTagName(validationResult.Value);

        return validationResult.Errors;
    }
}