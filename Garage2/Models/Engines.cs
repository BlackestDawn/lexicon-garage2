namespace Garage2.Models;

public interface IEngine
{
    int MaxPowerHP { get; }
    string Description { get; }
}

public readonly struct FuelEngine(int maxPowerHP, decimal displacementLiters, FuelTypes fuelType) : IEngine
{
    public int MaxPowerHP { get; } = maxPowerHP;
    public decimal DisplacementLiters {get; } = displacementLiters;
    public FuelTypes FuelType { get; } = fuelType;

    public string Description => $"{FuelType}, {DisplacementLiters}L, {MaxPowerHP}HP";
}

public readonly struct ElectricEngine(int maxPowerHP, decimal batteryCapacityKwh) : IEngine
{
    public int MaxPowerHP { get; } = maxPowerHP;
    public decimal BatteryCapacityKwh { get; } = batteryCapacityKwh;

    public string Description => $"Electric, {BatteryCapacityKwh} kWh, {MaxPowerHP}HP";
}
