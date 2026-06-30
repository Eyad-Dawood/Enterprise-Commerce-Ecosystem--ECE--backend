
namespace ECE.Domain.Warehouse.ProductBrands;

public static class ProductBrandErrors
{
    private const string ClassName = nameof(ProductBrand);

    static public readonly Error ProductBrandNameRequired =
    DomainCommonErrors.RequiredProp(ClassName, "ProductBrandName", "Product Brand Name");

    static public readonly Error InvalidProductBrandName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "ProductBrandName",
            "Product Brand Name",
            $"It must be between {ProductBrandName.MinLength} and {ProductBrandName.MaxLength} characters and contain only Arabic/English letters, English numbers, and spaces");
}