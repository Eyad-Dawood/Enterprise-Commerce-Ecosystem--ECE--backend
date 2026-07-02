namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class InventoryItemTransactionConfiguration : IEntityTypeConfiguration<InventoryItemTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryItemTransaction> builder)
    {
        builder.ToTable("InventoryItemTransactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.TransactionType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.BeforeState)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.AfterState)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.InventoryItemId)
            .IsRequired();

        builder.HasOne(x => x.InventoryItem)
            .WithMany()
            .HasForeignKey(x => x.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}