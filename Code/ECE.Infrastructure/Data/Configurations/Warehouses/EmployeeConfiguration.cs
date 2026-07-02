using ECE.Domain.WarehouseDomain.Warehouses.Employees;

namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CurrentWarehouseId)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasOne(x => x.CurrentWarehouse)
            .WithMany()
            .HasForeignKey(x => x.CurrentWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
