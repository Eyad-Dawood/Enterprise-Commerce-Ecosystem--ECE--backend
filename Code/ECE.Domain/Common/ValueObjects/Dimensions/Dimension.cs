namespace ECE.Domain.Common.ValueObjects.Dimensions;

public static class DimensionErrors
{
    private const string ClassName = nameof(Dimension);

    public static readonly Error InvalidDimension =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "Value",
            "Dimension Value",
            $"It must be between {Dimension.MinValue} and {Dimension.MaxValue}");
}

public record Dimension : ValueObject<decimal>
{
    public const decimal MinValue = 0.0001m;
    public const decimal MaxValue = 100000m;

    private Dimension(decimal value) : base(value)
    {
    }

    public static decimal Normalize(decimal value)
    {
        return decimal.Round(value, 4);
    }

    private static Result<decimal> Validate(decimal value)
    {
        if (value < MinValue || value > MaxValue)
            return DimensionErrors.InvalidDimension;

        return value;
    }

    public static Result<Dimension> Create(decimal value)
    {
        var normalized = Normalize(value);
        var validationResult = Validate(normalized);

        if (validationResult.IsSuccess)
            return new Dimension(validationResult.Value);

        return validationResult.Errors;
    }
}