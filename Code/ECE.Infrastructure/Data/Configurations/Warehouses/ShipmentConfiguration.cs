namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ConfigureAuditable();
        builder.ConfigureConcurrency();

        builder.ToTable("Shipments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ScheduledDate);

        builder.Property(x => x.ExternalLocation)
            .HasMaxLength(Shipment.ExternalLocationMaxLength)
            .IsRequired(false);

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.Direction)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}