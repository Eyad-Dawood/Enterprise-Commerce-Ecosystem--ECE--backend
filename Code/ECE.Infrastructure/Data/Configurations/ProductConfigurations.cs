namespace ECE.Infrastructure.Data.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ConfigureAuditable();
        builder.ConfigureConcurrency();

        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(p => p.Name, n =>
        {
            n.Property(p => p.Value)
            .HasColumnName("Name")
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

            n.HasIndex(x => x.Value)
            .IsUnique();
        });

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(Product.DescriptionMaxLength);

        builder.Property(p=>p.DefaultImageRelativePath)
            .IsRequired(false)
            .HasMaxLength(Product.DefaultImageRelativePathMaxLength);

        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Products)
            .UsingEntity(j =>
            {
                j.ToTable("ProductTagsMapping");
            });

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(p => p.Category)
           .WithMany(c => c.Products)
           .HasForeignKey(p => p.CategoryId)
           .OnDelete(DeleteBehavior.Restrict);

        
        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(p => p.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}