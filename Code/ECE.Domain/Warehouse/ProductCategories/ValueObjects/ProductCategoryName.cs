namespace ECE.Domain.Warehouse.ProductCategories.ValueObjects;

public record ProductCategoryName : ValueObject<string>
{
    public static readonly Regex ProductCategoryNameRegx =
        new(@"^[A-Za-z\u0600-\u06FF0-9 ]*$", RegexOptions.Compiled);

    public const int MinLength = 3;
    public const int MaxLength = 100;

    private ProductCategoryName(string Value) : base(Value)
    {
    }

    public static string Normalize(string value)
    {
        return ArabicNormalizer.Normalize(value.Trim());
    }
    private static Result<string> Validate(string value)
    {
        if (value.Length > MaxLength || value.Length < MinLength)
            return ProductCategoryErrors.InvalidProductCategoryName;

        if (!ProductCategoryNameRegx.IsMatch(value))
            return ProductCategoryErrors.InvalidProductCategoryName;

        return value;
    }

    public static Result<ProductCategoryName> Create(string? Value)
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ProductCategoryErrors.ProductCategoryNameRequired;

        var normalized = Normalize(Value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new ProductCategoryName(validationResult.Value);

        return validationResult.Errors;
    }
}