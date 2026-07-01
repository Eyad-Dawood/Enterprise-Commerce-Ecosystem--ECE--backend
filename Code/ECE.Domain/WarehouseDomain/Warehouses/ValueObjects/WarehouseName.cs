namespace ECE.Domain.WarehouseDomain.Warehouses.ValueObjects;

public record WarehouseName : ValueObject<string>
{
    public static readonly Regex WarehouseNameRegex =
      new(@"^[A-Za-z0-9\-]+$", RegexOptions.Compiled);

    public const int MinLength = 1;
    public const int MaxLength = 150;

    private WarehouseName(string value) : base(value)
    {
    }

    public static string Normalize(string value)
    {
        return value.Trim().ToLower();
    }

    private static Result<string> Validate(string value)
    {
        if (value.Length < MinLength || value.Length > MaxLength)
            return WarehouseErrors.InvalidWarehouseName;

        if (!WarehouseNameRegex.IsMatch(value))
            return WarehouseErrors.InvalidWarehouseName;

        return value;
    }

    public static Result<WarehouseName> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return WarehouseErrors.WarehouseNameRequired;

        var normalized = Normalize(value);
        var validation = Validate(normalized);

        if (validation.IsFailure)
            return validation.Errors;

        return new WarehouseName(validation.Value);
    }
}
