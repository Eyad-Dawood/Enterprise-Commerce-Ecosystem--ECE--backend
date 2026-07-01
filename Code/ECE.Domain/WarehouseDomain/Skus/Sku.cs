namespace ECE.Domain.WarehouseDomain.Skus;

public class Sku : Entity,IAuditableEntity,IConcurrencyEntity
{
    public const int DefaultImageRelativePathMaxLength = 1000;

    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }
    public byte[] RowVersion { get; private set; } = [];


    public Guid ProductId { get; private set; }
    public SkuCode Code { get; private set; } = null!;
    public SkuBarcode Barcode { get; private set; } = null!;
    public Price SellingPrice { get; private set; } = null!;
    public Weight? Weight { get; private set; }
    public Dimensions3D? Dimensions { get; private set; }
    public string? DefaultImageRelativePath { get; private set; }
    public SkuStatus Status { get; private set; }
    public bool IsAvailableForShipment { get; private set; }
    public int? ShelfLifeInDays { get; private set; } // if null , not expirable
    public StorageRequirements? StorageRequirements { get; private set; }

    private readonly List<SkuAttributeValue> _attributes = [];
    public IReadOnlyCollection<SkuAttributeValue> Attributes => _attributes;


    public Product? Product { get; private set; }

}