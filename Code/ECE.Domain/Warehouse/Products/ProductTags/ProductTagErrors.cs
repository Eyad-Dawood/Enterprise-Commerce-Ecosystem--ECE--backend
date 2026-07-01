
namespace ECE.Domain.Warehouse.Products.ProductTags;

public static class ProductTagErrors
{
    private const string ClassName = nameof(ProductTag);

    static public readonly Error InvalidProductTagName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "tag_name",
            "Tag Name",
            $"It must be between {ProductTagName.MinLength} and {ProductTagName.MaxLength} characters");

    static public readonly Error ProductTagNameRequired =
        DomainCommonErrors.RequiredProp(ClassName, "tag_name", "Tag Name");
}
