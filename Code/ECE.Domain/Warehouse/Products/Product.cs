
namespace ECE.Domain.Warehouse.Products;

public class Product : Entity, IAuditableEntity, IConcurrencyEntity
{
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
    public string[]? Tags { get; private set; }
    public ProductStatus Status { get; private set; }


    public ProductBrand? Brand { get; private set; }
    public ProductCategory? Category { get; private set; }
}