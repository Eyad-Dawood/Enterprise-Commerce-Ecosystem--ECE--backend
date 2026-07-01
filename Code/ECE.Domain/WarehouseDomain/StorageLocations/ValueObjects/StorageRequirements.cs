namespace ECE.Domain.WarehouseDomain.StorageLocations.ValueObjects;

public record StorageRequirements
{
    public TemperatureRequirement? Temperature { get; }
    public HumidityRequirement? Humidity { get; }
    public LightRequirement? Light { get; }
    public EnvironmentRequirement? Environment { get; }
    public SecurityRequirement? Security { get; }
    public SafetyRequirement? Safety { get; }
    public CertificationRequirement? Certification { get; }
    public StorageEquipmentRequirement? StorageEquipment { get; }
    public OrientationRequirement? Orientation { get; }

    private StorageRequirements(
        TemperatureRequirement? temperature,
        HumidityRequirement? humidity,
        LightRequirement? light,
        EnvironmentRequirement? environment,
        SecurityRequirement? security,
        SafetyRequirement? safety,
        CertificationRequirement? certification,
        StorageEquipmentRequirement? storageEquipment,
        OrientationRequirement? orientation)
    {
        Temperature = temperature;
        Humidity = humidity;
        Light = light;
        Environment = environment;
        Security = security;
        Safety = safety;
        Certification = certification;
        StorageEquipment = storageEquipment;
        Orientation = orientation;
    }

    public static Result<StorageRequirements> Create(
        TemperatureRequirement? temperature = null,
        HumidityRequirement? humidity = null,
        LightRequirement? light = null,
        EnvironmentRequirement? environment = null,
        SecurityRequirement? security = null,
        SafetyRequirement? safety = null,
        CertificationRequirement? certification = null,
        StorageEquipmentRequirement? storageEquipment = null,
        OrientationRequirement? orientation = null)
    {
        if (temperature.HasValue && !Enum.IsDefined(temperature.Value))
            return StorageRequirementErrors.InvalidTemperature;

        if (humidity.HasValue && !Enum.IsDefined(humidity.Value))
            return StorageRequirementErrors.InvalidHumidity;

        if (light.HasValue && !Enum.IsDefined(light.Value))
            return StorageRequirementErrors.InvalidLight;

        if (environment.HasValue && !Enum.IsDefined(environment.Value))
            return StorageRequirementErrors.InvalidEnvironment;

        if (security.HasValue && !Enum.IsDefined(security.Value))
            return StorageRequirementErrors.InvalidSecurity;

        if (safety.HasValue && !Enum.IsDefined(safety.Value))
            return StorageRequirementErrors.InvalidSafety;

        if (certification.HasValue && !Enum.IsDefined(certification.Value))
            return StorageRequirementErrors.InvalidCertification;

        if (storageEquipment.HasValue && !Enum.IsDefined(storageEquipment.Value))
            return StorageRequirementErrors.InvalidStorageEquipment;

        if (orientation.HasValue && !Enum.IsDefined(orientation.Value))
            return StorageRequirementErrors.InvalidOrientation;

        return new StorageRequirements(
            temperature,
            humidity,
            light,
            environment,
            security,
            safety,
            certification,
            storageEquipment,
            orientation);
    }
}