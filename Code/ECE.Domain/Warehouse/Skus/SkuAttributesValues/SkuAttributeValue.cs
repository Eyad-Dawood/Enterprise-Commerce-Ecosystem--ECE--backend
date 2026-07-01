namespace ECE.Domain.Warehouse.Skus.SkuAttributesValues;

public sealed class SkuAttributeValue : Entity
{
    public Guid SkuId { get; private set; }

    public Guid SkuAttributeDefinitionId { get; private set; }

    public SkuAttributeValueValue Value { get; private set; } = null!;

    public Sku? Sku { get; private set; }

    public SkuAttributeDefinition? Definition { get; private set; }
}