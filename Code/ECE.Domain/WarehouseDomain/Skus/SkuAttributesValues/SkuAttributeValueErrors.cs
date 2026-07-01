namespace ECE.Domain.WarehouseDomain.Skus.SkuAttributesValues;

public static class SkuAttributeValueErrors
{
    private const string ClassName = nameof(SkuAttributeValue);

    public static readonly Error InvalidSkuAttributeValue =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "value",
            "Value",
            $"It must be between {ValueObjects.SkuAttributeValueValue.MinLength} and {ValueObjects.SkuAttributeValueValue.MaxLength} characters and contain only Arabic/English letters, numbers, spaces, dots, commas, underscores, plus, minus, parentheses, and slashes");

    public static readonly Error SkuAttributeValueRequired =
        DomainCommonErrors.RequiredProp(ClassName, "value", "Value");
}
