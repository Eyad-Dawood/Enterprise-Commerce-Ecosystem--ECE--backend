namespace ECE.Domain.Warehouse.ProductCategories;

public class ProductCategory : Entity
{
    public ProductCategoryName Name { get; private set; } = null!;
}