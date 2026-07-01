namespace ECE.Domain.WarehouseDomain.Products;

static public class ProductErrors
{
    private const string ClassName = nameof(Product);

    static public readonly Error ProductNameRequired =
    DomainCommonErrors.RequiredProp(ClassName, "name", "Name");

    static public readonly Error InvalidProductName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "name",
            "Name",
            $"It must be between {ProductName.MinLength} and {ProductName.MaxLength} characters and contain only Arabic/English letters, English numbers, and spaces");
}