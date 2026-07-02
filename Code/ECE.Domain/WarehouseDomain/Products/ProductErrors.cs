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

    static public readonly Error InvalidProductDesciption =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "description",
            "Description",
            $"It must be less than {Product.DescriptionMaxLength} characters");

    static public readonly Error InvalidDefaultImageRelativePath =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "default_image_relative_path",
            "DefaultImageRelativePath",
            $"It must be less than {Product.DefaultImageRelativePathMaxLength} characters");

    static public readonly Error ProductCategoryIdRequired =
        DomainCommonErrors.RequiredProp(ClassName,
                                        "category_id",
                                        "CategoryId");

    static public readonly Error ProductBrandIdRequired =
        DomainCommonErrors.RequiredProp(ClassName,
                                        "brand_id",
                                        "BrandId");

    static public Error InvalidStatusTransition(ProductStatus fromStatus, ProductStatus toStatus) =>
        DomainCommonErrors.InvalidStateTransition(ClassName, fromStatus, toStatus);
}