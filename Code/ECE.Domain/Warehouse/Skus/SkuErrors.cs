namespace ECE.Domain.Warehouse.Skus;

static public class SkuErrors
{
    private const string ClassName = nameof(Sku);

    public static readonly Error InvalidSkuCode =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "code",
            "Code",
            $"It must be between {Skus.ValueObjects.SkuCode.MinLength} and {Skus.ValueObjects.SkuCode.MaxLength} characters and contain only uppercase letters, numbers, dots, underscores, hyphens, and slashes");

    public static readonly Error SkuCodeRequired =
        DomainCommonErrors.RequiredProp(ClassName, "code", "Code");

    public static readonly Error InvalidBarcode =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "barcode",
            "Barcode",
            "It must be exactly 8, 12, 13, or 14 digits");

    public static readonly Error BarcodeRequired =
        DomainCommonErrors.RequiredProp(ClassName, "barcode", "Barcode");
}
