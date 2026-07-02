namespace ECE.Domain.WarehouseDomain.InventoryItems.InventoryItemTransactions;

public class InventoryItemTransaction : Entity , IAuditableEntity
{
    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }


    public Guid InventoryItemId { get; private set; }
    public InventoryItemTransactionType TransactionType { get; private set; }
    public InventoryItemStatus BeforeState { get; set; }
    public InventoryItemStatus AfterState { get; set; }
    public Guid EmployeeId { get; private set; }


    public Employee? Employee { get; private set; }
    public InventoryItem? InventoryItem { get; private set; }

}