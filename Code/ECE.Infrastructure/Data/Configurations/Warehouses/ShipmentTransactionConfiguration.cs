namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class ShipmentTransactionConfiguration : IEntityTypeConfiguration<ShipmentTransaction>
{
    public void Configure(EntityTypeBuilder<ShipmentTransaction> builder)
    {
        builder.ConfigureAuditable();

        builder.ToTable("ShipmentTransactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ShipmentId)
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.BeforeStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.AfterStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(x => x.Shipment)
            .WithMany()
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}