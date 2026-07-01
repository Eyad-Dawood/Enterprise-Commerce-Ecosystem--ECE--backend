
namespace ECE.Domain.WarehouseDomain.StorageLocations;

public class StorageLocation : Entity
{
    public string Code { get; private set; } = null!;
    public Guid WarehouseId { get; private set; }
    public StorageRequirements StorageRequirements { get; private set; } = null!;
    public StorageLocationStatus Status { get; private set; }

    public Warehouse? Warehouse { get; private set; }
}