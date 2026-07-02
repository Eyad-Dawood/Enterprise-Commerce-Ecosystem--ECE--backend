using ECE.Domain.WarehouseDomain.InventoryItems;
using ECE.Domain.WarehouseDomain.Shipments.ShipmentLines;

namespace ECE.Domain.WarehouseDomain.Shipments.ShipmentInvntoryItems;

public class ShipmentInventoryItem : Entity, IAuditableEntity, IConcurrencyEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }
    public byte[] RowVersion { get; private set; } = [];

    public Guid ShipmentId { get; private set; }
    public Guid ShipmentLineId { get; private set; }
    public Guid InventoryItemId { get; private set; }
    public Guid EmployeeId { get; private set; }


    public Employee? Employee { get; private set; }
    public Shipment? Shipment { get; private set; }
    public ShipmentLine? ShipmentLine { get; private set; }
    public InventoryItem? InventoryItem { get; private set; }
}