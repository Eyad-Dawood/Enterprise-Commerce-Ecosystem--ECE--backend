namespace ECE.Domain.Warehouse.StorageLocations;

public static class StorageRequirementErrors
{
    private const string ClassName = nameof(StorageRequirements);

    public static readonly Error InvalidTemperature =
        DomainCommonErrors.InvalidProp(ClassName, "temperature", "Temperature", "It must be a valid temperature requirement");

    public static readonly Error InvalidHumidity =
        DomainCommonErrors.InvalidProp(ClassName, "humidity", "Humidity", "It must be a valid humidity requirement");

    public static readonly Error InvalidLight =
        DomainCommonErrors.InvalidProp(ClassName, "light", "Light", "It must be a valid light requirement");

    public static readonly Error InvalidEnvironment =
        DomainCommonErrors.InvalidProp(ClassName, "environment", "Environment", "It must be a valid environment requirement");

    public static readonly Error InvalidSecurity =
        DomainCommonErrors.InvalidProp(ClassName, "security", "Security", "It must be a valid security requirement");

    public static readonly Error InvalidSafety =
        DomainCommonErrors.InvalidProp(ClassName, "safety", "Safety", "It must be a valid safety requirement");

    public static readonly Error InvalidCertification =
        DomainCommonErrors.InvalidProp(ClassName, "certification", "Certification", "It must be a valid certification requirement");

    public static readonly Error InvalidStorageEquipment =
        DomainCommonErrors.InvalidProp(ClassName, "storage_equipment", "StorageEquipment", "It must be a valid storage equipment requirement");

    public static readonly Error InvalidOrientation =
        DomainCommonErrors.InvalidProp(ClassName, "orientation", "Orientation", "It must be a valid orientation requirement");
}
