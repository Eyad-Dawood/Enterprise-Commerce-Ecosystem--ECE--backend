namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("ProductTags");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(t => t.TagName, n =>
        {
            n.Property(p => p.Value)
                .HasColumnName("TagName")
                .HasMaxLength(ProductTagName.MaxLength)
                .IsRequired();

            n.HasIndex(p => p.Value)
                .IsUnique();
        });

        builder.Navigation(t => t.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}