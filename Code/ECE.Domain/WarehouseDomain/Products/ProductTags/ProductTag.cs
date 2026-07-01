namespace ECE.Domain.WarehouseDomain.Products.ProductTags;

public class ProductTag : Entity
{
    public ProductTagName TagName { get; set; } = null!;


    private readonly List<Product> _products = [];

    public IReadOnlyCollection<Product> Products => _products;
}