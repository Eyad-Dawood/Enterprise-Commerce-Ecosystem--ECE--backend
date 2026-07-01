namespace ECE.Domain.WarehouseDomain.Warehouses;

static public class WarehouseErrors
{
    private const string ClassName = nameof(Warehouse);

    public static readonly Error InvalidWarehouseName =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "name",
            "Name",
            $"It must be between {Warehouses.ValueObjects.WarehouseName.MinLength} and {Warehouses.ValueObjects.WarehouseName.MaxLength} characters and contain only letters, numbers, and hyphens");

    public static readonly Error WarehouseNameRequired =
        DomainCommonErrors.RequiredProp(ClassName, "name", "Name");
}