
namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class ShipmentLineConfiguration : IEntityTypeConfiguration<ShipmentLine>
{
    public void Configure(EntityTypeBuilder<ShipmentLine> builder)
    {
        builder.ConfigureAuditable();
        builder.ConfigureConcurrency();

        builder.ToTable("ShipmentLines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ShipmentId)
            .IsRequired();

        builder.Property(x => x.SkuId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne(x => x.Shipment)
            .WithMany()
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Sku)
            .WithMany()
            .HasForeignKey(x => x.SkuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ShipmentId, x.SkuId })
            .IsUnique();
    }
}