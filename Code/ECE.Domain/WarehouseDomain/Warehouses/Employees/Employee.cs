namespace ECE.Domain.WarehouseDomain.Warehouses.Employees;

public class Employee : Entity , IAuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }

    public string UserId { get; private set; } = null!;
    public Guid CurrentWarehouseId { get; private set; }

    public Warehouse? CurrentWarehouse { get; private set; }
}