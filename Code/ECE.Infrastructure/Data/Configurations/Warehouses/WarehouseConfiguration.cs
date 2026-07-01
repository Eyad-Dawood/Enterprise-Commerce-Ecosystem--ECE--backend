namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedNever();

        builder.Property(w => w.Name)
            .HasConversion(
                name => name.Value,
                value => WarehouseName.Create(value).Value)
            .HasMaxLength(WarehouseName.MaxLength) 
            .IsRequired();

       builder.HasIndex(w=>w.Name)
            .IsUnique();
    
    }
}