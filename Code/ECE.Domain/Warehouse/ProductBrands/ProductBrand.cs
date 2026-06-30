namespace ECE.Domain.Warehouse.ProductBrands;

public class ProductBrand : Entity
{
    public ProductBrandName Name { get; private set; } = null!;
}