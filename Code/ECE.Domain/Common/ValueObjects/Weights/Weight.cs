namespace ECE.Domain.Common.ValueObjects.Weights;

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

        return new Weight(decimal.Round(value, 3), unit);
    }
}