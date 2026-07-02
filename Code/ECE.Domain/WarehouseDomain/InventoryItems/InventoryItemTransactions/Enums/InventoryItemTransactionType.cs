namespace ECE.Domain.WarehouseDomain.InventoryItems.InventoryItemTransactions.Enums;

public enum InventoryItemTransactionType
{
    Receive = 0,
    Dispatch = 1,
    Damage = 2,
    Adjustment = 3,
    Move = 4,
    Expire = 5,
    Return = 6,
    MakeAvailable = 7
}