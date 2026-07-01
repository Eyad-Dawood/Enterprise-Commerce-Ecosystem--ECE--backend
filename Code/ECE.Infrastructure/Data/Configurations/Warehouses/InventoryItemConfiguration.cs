
namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ConfigureAuditable();
        builder.ConfigureConcurrency();

        builder.ToTable("InventoryItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.StorageLocationId)
            .IsRequired();

        builder.Property(i => i.SkuId)
            .IsRequired();

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(20)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(
                "'INV-' + RIGHT(REPLICATE('0', 10) + CAST(NEXT VALUE FOR InventoryItemSerialSequence AS varchar(10)), 10)");

        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(i => i.BatchId)
            .IsRequired();


        builder.HasOne(i => i.Sku)
            .WithMany()
            .HasForeignKey(i => i.SkuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.StorageLocation)
            .WithMany() 
            .HasForeignKey(i => i.StorageLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Batch)
            .WithMany()
            .HasForeignKey(i => i.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SerialNumber)
            .IsUnique();

        builder.Property(x => x.SerialNumber)
    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Throw);
    }
}