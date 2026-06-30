namespace ECE.Domain.Warehouse.ProductBrands;

public class ProductBrand : Entity
{
    public ProductBrandName Name { get; private set; } = null!;


    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products;
}