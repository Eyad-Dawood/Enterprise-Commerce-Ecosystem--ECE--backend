namespace ECE.Infrastructure.Data.Configurations.Warehouse;

public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.ToTable("ProductBrands");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(b => b.Name, n =>
        {
            n.Property(p => p.Value)
                .HasColumnName("Name")
                .HasMaxLength(ProductBrandName.MaxLength)
                .IsRequired();

            n.HasIndex(p => p.Value)
                .IsUnique();
        });

        builder.Navigation(b => b.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
