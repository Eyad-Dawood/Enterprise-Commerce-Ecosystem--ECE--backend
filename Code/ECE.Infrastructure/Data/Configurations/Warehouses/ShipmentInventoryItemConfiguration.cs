
namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class ShipmentInventoryItemConfiguration : IEntityTypeConfiguration<ShipmentInventoryItem>
{
    public void Configure(EntityTypeBuilder<ShipmentInventoryItem> builder)
    {
        builder.ConfigureAuditable();
        builder.ConfigureConcurrency();

        builder.ToTable("ShipmentInventoryItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ShipmentId)
            .IsRequired();

        builder.Property(x => x.ShipmentLineId)
            .IsRequired();

        builder.Property(x => x.InventoryItemId)
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.HasOne(x => x.Shipment)
            .WithMany()
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ShipmentLine)
            .WithMany()
            .HasForeignKey(x => x.ShipmentLineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.InventoryItem)
            .WithMany()
            .HasForeignKey(x => x.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new {x.ShipmentId , x.InventoryItemId })
            .IsUnique();
    }
}