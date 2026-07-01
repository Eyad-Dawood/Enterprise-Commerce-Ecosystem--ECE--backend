using ECE.Domain.WarehouseDomain.InventoryItemsBatches;

namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class InventoryItemBatchConfiguration : IEntityTypeConfiguration<InventoryItemsBatch>
{
    public void Configure(EntityTypeBuilder<InventoryItemsBatch> builder)
    {
        builder.ToTable("InventoryItemsBatches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Lot)
            .IsRequired(false)
            .HasMaxLength(InventoryItemsBatch.LotMaxLength);

        builder
            .HasIndex(b => b.Lot);
    }
}