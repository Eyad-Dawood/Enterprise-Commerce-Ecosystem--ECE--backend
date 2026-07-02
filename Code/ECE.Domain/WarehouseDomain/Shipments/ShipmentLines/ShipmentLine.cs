namespace ECE.Domain.WarehouseDomain.Shipments.ShipmentLines;

public class ShipmentLine : Entity , IAuditableEntity, IConcurrencyEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }
    public byte[] RowVersion { get; private set; } = [];


    public Guid ShipmentId { get; private set; }
    public Guid SkuId { get; private set; }
    public int Quantity { get; private set; }


    public Sku? Sku { get; private set; }
    public Shipment? Shipment { get; private set; }
}