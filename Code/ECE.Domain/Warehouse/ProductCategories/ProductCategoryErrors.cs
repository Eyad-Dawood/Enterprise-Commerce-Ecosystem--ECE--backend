namespace ECE.Domain.Warehouse.ProductCategories;

public static class ProductCategoryErrors
{
    private const string ClassName = nameof(ProductCategory);

    static public readonly Error ProductCategoryNameRequired =
    DomainCommonErrors.RequiredProp(ClassName, "category_name", "Category Name");

    static public readonly Error InvalidProductCategoryName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "category_name",
            "Category Name",
            $"It must be between {ProductCategoryName.MinLength} and {ProductCategoryName.MaxLength} characters and contain only Arabic/English letters, English numbers, and spaces");
}