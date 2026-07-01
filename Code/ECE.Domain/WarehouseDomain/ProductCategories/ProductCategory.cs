
namespace ECE.Domain.WarehouseDomain.ProductCategories;

public class ProductCategory : Entity
{
    public ProductCategoryName Name { get; private set; } = null!;

    private readonly List<Product> _products = [];

    public IReadOnlyCollection<Product> Products => _products;
}