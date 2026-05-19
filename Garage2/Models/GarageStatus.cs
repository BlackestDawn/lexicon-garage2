using Garage2.Models.Enums;

namespace Garage2.Models;

public record GarageStatus
(
    int Capacity,
    int Used,
    IReadOnlyDictionary<VehicleTypes, int> TypesCount
);
