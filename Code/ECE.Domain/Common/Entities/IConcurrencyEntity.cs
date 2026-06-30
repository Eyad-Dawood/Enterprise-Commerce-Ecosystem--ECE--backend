namespace ECE.Domain.Common.Entities;

public interface IConcurrencyEntity
{
    byte[] RowVersion { get; }
}

