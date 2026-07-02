
namespace ECE.Domain.WarehouseDomain.Shipments.ShipmentTransactions;

public class ShipmentTransaction : Entity, IAuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }

    public Guid ShipmentId { get; private set; }
    public ShipmentStatus BeforeStatus { get; private set; }
    public ShipmentStatus AfterStatus { get; private set; }
    public Guid EmployeeId { get; private set; }
    public ShipmentTransactionType Type { get; private set; }


    public Employee? Employee { get; private set; }
    public Shipment? Shipment { get; private set; }

}