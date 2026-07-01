namespace ECE.Infrastructure.Data.Configurations.Warehouses;


public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocation>
{
    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.ToTable("StorageLocations");

        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.Id)
            .ValueGeneratedNever();

        builder.Property(sl => sl.Code)
            .IsRequired()
            .HasMaxLength(StorageLocation.CodeMaxLength);

        builder.Property(sl => sl.WarehouseId)
            .IsRequired();


        builder.OwnsOne(sl => sl.StorageRequirements, sr =>
        {
            sr.Property(x => x.Temperature)
                .HasColumnName("TemperatureRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Humidity)
                .HasColumnName("HumidityRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Light)
                .HasColumnName("LightRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Environment)
                .HasColumnName("EnvironmentRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Security)
                .HasColumnName("SecurityRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Safety)
                .HasColumnName("SafetyRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Certification)
                .HasColumnName("CertificationRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.StorageEquipment)
                .HasColumnName("StorageEquipmentRequirement")
                .HasConversion<int?>();

            sr.Property(x => x.Orientation)
                .HasColumnName("OrientationRequirement")
                .HasConversion<int?>();
        });


        builder.Property(sl => sl.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasOne(sl => sl.Warehouse)
            .WithMany() 
            .HasForeignKey(sl => sl.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    
        builder.HasIndex(p=>new { p.Code , p.WarehouseId})
            .IsUnique();
    }
}