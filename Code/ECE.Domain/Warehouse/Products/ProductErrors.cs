namespace ECE.Domain.Warehouse.Products;

static public class ProductErrors
{
    private const string ClassName = nameof(Product);

    static public readonly Error ProductNameRequired =
    DomainCommonErrors.RequiredProp(ClassName, "ProductName", "Product Name");

    static public readonly Error InvalidProductName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "ProductName",
            "Product Name",
            $"It must be between {ProductName.MinLength} and {ProductName.MaxLength} characters and contain only Arabic/English letters, English numbers, and spaces");
}