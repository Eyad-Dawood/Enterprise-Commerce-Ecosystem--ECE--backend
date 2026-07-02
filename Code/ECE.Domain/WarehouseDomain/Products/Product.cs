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


    private Product() { }

    private Product(ProductName name,
                   string? description,
                   Guid categoryId,
                   Guid? brandId,
                   string? defaultImageRelativePath,
                   List<ProductTag> tags)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        BrandId = brandId;
        DefaultImageRelativePath = defaultImageRelativePath;
        _tags = tags;

        Status = ProductStatus.Active;
    }


    private static bool IsValidName(ProductName name)
        => name is not null;

    private static bool IsValidCategoryId(Guid categoryId)
        => categoryId != Guid.Empty;


    private static bool IsValidBrandId(Guid? brandId)
        => !brandId.HasValue || brandId.Value != Guid.Empty;

    private static bool IsValidDescription(string? description)
        => description is null || description.Length <= DescriptionMaxLength;
    

    private static bool IsValidDefaultImageRelativePath(string? defaultImageRelativePath)
        => defaultImageRelativePath is null ||
           defaultImageRelativePath.Length <= DefaultImageRelativePathMaxLength;


    public static Result<Product> Create(ProductName name,
                                         string? description,
                                         Guid categoryId,
                                         Guid? brandId,
                                         string? defaultImageRelativePath,
                                         IReadOnlyCollection<ProductTag> tags)
    {
        if (!IsValidName(name))
            return ProductErrors.ProductNameRequired;
        if (!IsValidCategoryId(categoryId))
            return ProductErrors.ProductCategoryIdRequired;
        if (!IsValidBrandId(brandId))
            return ProductErrors.ProductBrandIdRequired;
        if (!IsValidDescription(description))
            return ProductErrors.InvalidProductDesciption;
        if (!IsValidDefaultImageRelativePath(defaultImageRelativePath))
            return ProductErrors.InvalidDefaultImageRelativePath;

        tags ??= [];


        return new Product(name,
                           description,
                           categoryId,
                           brandId,
                           defaultImageRelativePath,
                           [.. tags.Distinct()]);

    }

    public Result<Updated> Update(ProductName name,
                                 string? description,
                                 Guid categoryId,
                                 Guid? brandId,
                                 string? defaultImageRelativePath,
                                 IReadOnlyCollection<ProductTag> tags)
    {
        if (!IsValidName(name))
            return ProductErrors.ProductNameRequired;
        if (!IsValidCategoryId(categoryId))
            return ProductErrors.ProductCategoryIdRequired;
        if (!IsValidBrandId(brandId))
            return ProductErrors.ProductBrandIdRequired;
        if (!IsValidDescription(description))
            return ProductErrors.InvalidProductDesciption;
        if (!IsValidDefaultImageRelativePath(defaultImageRelativePath))
            return ProductErrors.InvalidDefaultImageRelativePath;
        tags ??= [];
        Name = name;
        Description = description;
        CategoryId = categoryId;
        BrandId = brandId;
        DefaultImageRelativePath = defaultImageRelativePath;
        _tags.Clear();
        _tags.AddRange(tags.Distinct());
        return Result.Updated;
    }

    private bool CanTransitionToStatus(ProductStatus newStatus)
    {
        return Status switch
        {
            ProductStatus.Active => newStatus is ProductStatus.Inactive,
            ProductStatus.Inactive => newStatus is ProductStatus.Active,
            _ => false
        };
    }

    private Result<Updated> UpdateStatus(ProductStatus newStatus)
    {
        if (!CanTransitionToStatus(newStatus))
            return ProductErrors.InvalidStatusTransition(Status, newStatus);


        Status = newStatus;
        return Result.Updated;
    }

    public Result<Updated> MarkAsActive()
    {
        return UpdateStatus(ProductStatus.Active);
    }

    public Result<Updated> MarkAsInactive()
    {
        return UpdateStatus(ProductStatus.Inactive);
    }

}
