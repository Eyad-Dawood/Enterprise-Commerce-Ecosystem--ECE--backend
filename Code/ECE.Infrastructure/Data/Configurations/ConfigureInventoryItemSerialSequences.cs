namespace ECE.Infrastructure.Data.Configurations;

public static class ConfigureInventoryItemSerialSequences
{
    public static void ConfigureSequences(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<long>("InventoryItemSerialSequence")
            .StartsAt(1)
            .IncrementsBy(1);
    }
}