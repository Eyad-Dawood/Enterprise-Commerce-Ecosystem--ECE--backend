namespace ECE.Domain.WarehouseDomain.Shipments;

public class Shipment : Entity , IAuditableEntity , IConcurrencyEntity
{
    public const int ExternalLocationMaxLength = 1000;

    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }
    public byte[] RowVersion { get; private set; } = [];

    public DateTimeOffset? ScheduledDate { get; private set; } // has different meaning based on Incoming or Outgoing , but must be null if ScheduleType Is not Planned
    public string? ExternalLocation { get; private set; } // either it came from or going to (destination or source)
    public Guid WarehouseId { get; private set; }
    public ShipmentDirection Direction { get; private set; } 
    public ShipmentStatus Status { get; private set; }
    public Warehouse? Warehouse { get; private set; }
}