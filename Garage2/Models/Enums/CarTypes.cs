using System.ComponentModel;

namespace Garage2.Models.Enums;

public enum CarTypes
{
    SUV,
    Sedan,
    Hatchback,
    [Description("Pickup Truck")]
    Pickup,
    [Description("Minivan / MPV")]
    Minivan,
    Coupe,
    Convertible,
    [Description("Station Wagon")]
    Station,
    [Description("Sports Car")]
    Sport,
}
