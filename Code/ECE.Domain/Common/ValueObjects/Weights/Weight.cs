namespace ECE.Domain.Common.ValueObjects.Weights;

public static class WeightErrors
{
    private const string ClassName = nameof(Weight);

    public static readonly Error InvalidWeight =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "Value",
            "Weight Value",
            $"It must be between {Weight.MinValue} and {Weight.MaxValue}");

    public static readonly Error InvalidWeightUnit =
        DomainCommonErrors.InvalidProp(
            ClassName,
            "Unit",
            "Weight Unit",
            "It must be a valid weight unit");
}

public record Weight
{
    public decimal Value { get; }
    public WeightUnit Unit { get; }

    private Weight(decimal value, WeightUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    public const decimal MinValue = 0m;
    public const decimal MaxValue = 100000m;

    public static Result<Weight> Create(decimal value, WeightUnit unit)
    {
        if (value < MinValue || value > MaxValue)
            return WeightErrors.InvalidWeight;

        if(!Enum.IsDefined(unit))
            return WeightErrors.InvalidWeightUnit;

        return new Weight(decimal.Round(value, 3), unit);
    }
}