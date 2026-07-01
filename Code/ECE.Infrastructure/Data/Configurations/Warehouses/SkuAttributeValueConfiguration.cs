namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class SkuAttributeValueConfiguration : IEntityTypeConfiguration<SkuAttributeValue>
{
    public void Configure(EntityTypeBuilder<SkuAttributeValue> builder)
    {
        builder.ToTable("SkuAttributeValues");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.SkuId)
            .IsRequired();

        builder.Property(x => x.SkuAttributeDefinitionId)
            .IsRequired();

        builder.OwnsOne(x => x.Value, value =>
        {
            value.Property(v => v.Value)
                .HasColumnName("Value")
                .HasMaxLength(SkuAttributeValueValue.MaxLength)
                .IsRequired();
        });

        builder.HasIndex(x => new
        {
            x.SkuId,
            x.SkuAttributeDefinitionId
        })
        .IsUnique();

        builder.HasOne(x => x.Sku)
            .WithMany(s => s.Attributes)
            .HasForeignKey(x => x.SkuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Definition)
            .WithMany()
            .HasForeignKey(x => x.SkuAttributeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}