
namespace ECE.Domain.WarehouseDomain.StorageLocations;

public class StorageLocation : Entity
{
    public const int CodeMaxLength = 50;

    public string Code { get; private set; } = null!;
    public Guid WarehouseId { get; private set; }
    public StorageRequirements StorageRequirements { get; private set; } = null!;
    public StorageLocationStatus Status { get; private set; }
    public int PickSequence {  get; private set; }

    public Warehouse? Warehouse { get; private set; }
}