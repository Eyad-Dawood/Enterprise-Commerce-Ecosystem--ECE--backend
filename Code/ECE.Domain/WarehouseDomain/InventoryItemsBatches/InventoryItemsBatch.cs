namespace ECE.Domain.WarehouseDomain.InventoryItemsBatches;

public class InventoryItemsBatch: Entity , IAuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }

    public DateTimeOffset? ManufacturingDate { get; private set; }
    public DateTimeOffset? ExpirationDate { get; private set; }
    public string? Lot { get; private set; }

    // public Guid? ShipmentId {  get; private set; }

    //public Shipment? Shipment { get; private set; }

}