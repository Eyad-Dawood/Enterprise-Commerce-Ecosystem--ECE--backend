
namespace ECE.Domain.Warehouse.ProductCategories;

public class ProductCategory : Entity
{
    public ProductCategoryName Name { get; private set; } = null!;

    private readonly List<Product> _products = [];

    public IReadOnlyCollection<Product> Products => _products;
}