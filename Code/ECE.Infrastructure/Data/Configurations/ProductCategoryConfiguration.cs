
namespace ECE.Infrastructure.Data.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(c => c.Name, n =>
        {
            n.Property(p => p.Value)
                .HasColumnName("Name")
                .HasMaxLength(ProductCategoryName.MaxLength)
                .IsRequired();

            n.HasIndex(p => p.Value)
                .IsUnique();
        });

        builder.Navigation(c => c.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}