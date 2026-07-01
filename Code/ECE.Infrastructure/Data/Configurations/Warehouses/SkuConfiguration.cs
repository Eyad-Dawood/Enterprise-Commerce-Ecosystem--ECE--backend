namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class SkuConfiguration : IEntityTypeConfiguration<Sku>
{
    public void Configure(EntityTypeBuilder<Sku> builder)
    {
        builder.ConfigureAuditable();
        builder.ConfigureConcurrency();

        builder.ToTable("Skus");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.ProductId)
            .IsRequired();

        builder.Property(s => s.Code)
     .HasConversion(
         code => code.Value,
         value => SkuCode.Create(value).Value)
     .HasMaxLength(SkuCode.MaxLength)
     .IsRequired();

        builder.Property(s => s.Barcode)
            .HasConversion(
                barcode => barcode.Value,
                value => SkuBarcode.Create(value).Value)
            .HasMaxLength(SkuBarcode.MaxLength)
            .IsRequired();


        builder.OwnsOne(s => s.SellingPrice, p =>
        {
            p.Property(pr => pr.Amount)
                .HasColumnName("SellingPriceAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            p.Property(pr => pr.Currency)
                .HasColumnName("SellingPriceCurrency")
                .HasConversion<int>()
                .IsRequired();
        });

        builder.OwnsOne(s => s.Weight, w =>
        {
            w.Property(we => we.Value)
                .HasColumnName("WeightValue")
                .HasPrecision(18, 4);

            w.Property(we => we.Unit)
                .HasColumnName("WeightUnit")
                .HasConversion<int>();
        });

        builder.OwnsOne(s => s.Dimensions, dimensions =>
        {
            dimensions.OwnsOne(d => d.Length, length =>
            {
                length.Property(x => x.Value)
                    .HasColumnName("DimensionLength")
                    .HasPrecision(18, 4)
                    .IsRequired();
            });

            dimensions.OwnsOne(d => d.Width, width =>
            {
                width.Property(x => x.Value)
                    .HasColumnName("DimensionWidth")
                    .HasPrecision(18, 4)
                    .IsRequired();
            });

            dimensions.OwnsOne(d => d.Height, height =>
            {
                height.Property(x => x.Value)
                    .HasColumnName("DimensionHeight")
                    .HasPrecision(18, 4)
                    .IsRequired();
            });

            dimensions.Property(d => d.LengthUnit)
                .HasColumnName("DimensionLengthUnit")
                .HasConversion<int>()
                .IsRequired();
        });

        builder.Property(s => s.DefaultImageRelativePath)
            .IsRequired(false)
            .HasMaxLength(Sku.DefaultImageRelativePathMaxLength); 

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.IsAvailableForShipment)
            .IsRequired();

        builder.Property(s => s.ShelfLifeInDays)
            .IsRequired(false);

        builder.OwnsOne(s => s.StorageRequirements, sr =>
        {
            sr.Property(req => req.Temperature)
                .HasColumnName("RequiredTemperature")
                .HasConversion<int>();
            
            sr.Property(req => req.Humidity)
                .HasColumnName("RequiredHumidity")
                .HasConversion<int>();

            sr.Property(req => req.Light)
                .HasColumnName("RequiredLight")
                .HasConversion<int>();

            sr.Property(req => req.Environment)
                .HasColumnName("RequiredEnvironment")
                .HasConversion<int>();

            sr.Property(req => req.Security)
                .HasColumnName("RequiredSecurity")
                .HasConversion<int>();

            sr.Property(req => req.Safety)
                .HasColumnName("RequiredSafety")
                .HasConversion<int>();

            sr.Property(req => req.Certification)
                .HasColumnName("RequiredCertification")
                .HasConversion<int>();

            sr.Property(req => req.StorageEquipment)
                .HasColumnName("RequiredStorageEquipment")
                .HasConversion<int>();

            sr.Property(req => req.Orientation)
                .HasColumnName("RequiredOrientation")
                .HasConversion<int>();
        });

        builder.HasMany(s => s.Attributes)
            .WithOne(a => a.Sku)
            .HasForeignKey(a => a.SkuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Product)
            .WithMany(p => p.Skus)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(s => s.Attributes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => new
        {
            x.ProductId,
            x.Code
        })
        .IsUnique();

        builder.HasIndex(x => x.Barcode)
            .IsUnique();
    }
}