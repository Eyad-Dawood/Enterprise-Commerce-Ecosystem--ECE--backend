namespace ECE.Domain.Warehouse.Skus.SkuAttributesDefinitions;

public static class SkuAttributeDefinitionErrors
{
    private const string ClassName = nameof(SkuAttributeDefinition);

    public static readonly Error InvalidSkuAttributeDefinitionName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "name",
            "Name",
            $"It must be between {ValueObjects.SkuAttributeDefinitionName.MinLength} and {ValueObjects.SkuAttributeDefinitionName.MaxLength} characters and contain only English letters, numbers, and spaces");

    public static readonly Error SkuAttributeDefinitionNameRequired =
        DomainCommonErrors.RequiredProp(ClassName, "name", "Name");
}
