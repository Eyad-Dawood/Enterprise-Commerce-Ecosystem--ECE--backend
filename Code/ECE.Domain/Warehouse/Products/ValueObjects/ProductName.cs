
namespace ECE.Domain.Warehouse.Products.ValueObjects;

public record ProductName : ValueObject<string>
{
    public static readonly Regex ProductNameRegx =
        new(@"^[A-Za-z\u0600-\u06FF0-9 ]*$", RegexOptions.Compiled);

    public const int MinLength = 3;
    public const int MaxLength = 300;

    private ProductName(string Value) : base(Value)
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
            return ProductErrors.InvalidProductName;

        if(!ProductNameRegx.IsMatch(value))
            return ProductErrors.InvalidProductName;

        return value;
    }

    public static Result<ProductName> Create(string? Value) 
    {
        if (string.IsNullOrWhiteSpace(Value))
            return ProductErrors.ProductNameRequired;

        var normalized = Normalize(Value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new ProductName(validationResult.Value);

        return validationResult.Errors;
    }
}