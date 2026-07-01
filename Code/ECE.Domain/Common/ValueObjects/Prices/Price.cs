namespace ECE.Domain.Common.ValueObjects.Prices;

public record Price 
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Price(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public const decimal MinValue = 0m;
    public const decimal MaxValue = 999999999.99m;

    public static Result<Price> Create(decimal amount, Currency currency)
    {
        if (amount < MinValue || amount > MaxValue)
            return PriceErrors.InvalidPrice;

        if (!Enum.IsDefined(currency))
            return PriceErrors.InvalidCurrency;

        return new Price(decimal.Round(amount, 2), currency);
    }
}