namespace ECE.Domain.WarehouseDomain.InventoryItems;

public class InventoryItem : Entity, IAuditableEntity, IConcurrencyEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }
    public byte[] RowVersion { get; private set; } = [];

    public Guid StorageLocationId { get; private set; }
    public Guid SkuId { get; private set; }
    public string SerialNumber { get; private set; } = null!;
    public InventoryItemStatus Status { get; private set; }
    public Guid BatchId { get; private set; }


    public Sku? Sku { get; private set; }
    public StorageLocation? StorageLocation { get; private set; }
    public InventoryItemsBatch? Batch { get; private set;}

}