using ECE.Domain.WarehouseDomain.Skus;

namespace ECE.Domain.WarehouseDomain.Products;

public class Product : Entity, IAuditableEntity, IConcurrencyEntity
{
    public const int DescriptionMaxLength = 1000;
    public const int DefaultImageRelativePathMaxLength = 1000;

    public DateTimeOffset CreatedAtUtc { get; }
    public string? CreatedBy { get; }
    public DateTimeOffset LastModifiedUtc { get; }
    public string? LastModifiedBy { get; }
    public byte[] RowVersion { get; private set; } = [];


    public ProductName Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid? BrandId { get; private set; }
    public string? DefaultImageRelativePath { get; private set; }

    private readonly List<ProductTag> _tags = [];
    public IReadOnlyCollection<ProductTag> Tags => _tags;

    public ProductStatus Status { get; private set; }


    public ProductBrand? Brand { get; private set; }
    public ProductCategory? Category { get; private set; }


    private readonly List<Sku> _skus = [];
    public IReadOnlyCollection<Sku> Skus => _skus;
}