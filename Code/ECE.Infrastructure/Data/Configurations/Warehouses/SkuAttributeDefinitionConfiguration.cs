namespace ECE.Infrastructure.Data.Configurations.Warehouses;

public class SkuAttributeDefinitionConfiguration : IEntityTypeConfiguration<SkuAttributeDefinition>
{
    public void Configure(EntityTypeBuilder<SkuAttributeDefinition> builder)
    {
        builder.ToTable("SkuAttributeDefinitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(x => x.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(SkuAttributeDefinitionName.MaxLength)
                .IsRequired();

            name.HasIndex(n => n.Value)
                .IsUnique();
        });
    }
}