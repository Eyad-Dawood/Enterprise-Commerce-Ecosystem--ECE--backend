namespace ECE.Domain.Common.ValueObjects.Dimensions;

public static class Dimension3DErrors
{
    private const string ClassName = nameof(Dimensions3D);

    public static readonly Error InvalidLengthUnit =
    DomainCommonErrors.InvalidProp(
        ClassName,
        "LengthUnit",
        "Length Unit",
        "It must be a valid length unit");
}

public record Dimensions3D
{
    public Dimension Length { get; }
    public Dimension Width { get; }
    public Dimension Height { get; }
    public LengthUnit LengthUnit { get; }

    private Dimensions3D(
        Dimension length,
        Dimension width,
        Dimension height,
        LengthUnit lengthUnit)
    {
        Length = length;
        Width = width;
        Height = height;
        LengthUnit = lengthUnit;
    }

    public static Result<Dimensions3D> Create(
        Dimension length,
        Dimension width,
        Dimension height,
        LengthUnit lengthUnit)
    {
        if (!Enum.IsDefined(lengthUnit))
            return Dimension3DErrors.InvalidLengthUnit;

        return new Dimensions3D(length, width, height, lengthUnit);
    }

    public static Result<Dimensions3D> Create(
    decimal length,
    decimal width,
    decimal height,
    LengthUnit lengthUnit)
    {
        var lengthResult = Dimension.Create(length);
        if (lengthResult.IsFailure)
            return lengthResult.Errors;

        var widthResult = Dimension.Create(width);
        if (widthResult.IsFailure)
            return widthResult.Errors;

        var heightResult = Dimension.Create(height);
        if (heightResult.IsFailure)
            return heightResult.Errors;

        return Create(
            lengthResult.Value,
            widthResult.Value,
            heightResult.Value,
            lengthUnit);
    }

}