
using System.Text;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2Testing;

public class VehicleTests
{
    [Fact]
    public void Vehicle_MinimalDescription()
    {
        Vehicle vehicle = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White");

        string expected = "Licence: EV0001, Type: Car";

        Assert.Equal(expected, vehicle.MinimalDescription());
    }

    [Fact]
    public void Vehicle_FullDescription()
    {
        Vehicle vehicle = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White");

        StringBuilder expected = new();

        expected.AppendLine("Licence: EV0001, Type: Car");
        expected.AppendLine("Color: White");
        expected.AppendLine("Engine: Electric, 100.0 kWh, 408HP");

        Assert.Equal(expected.ToString(), vehicle.FullDescription());
    }

    [Fact]
    public void Vehicle_CarOverrided_FullDescription()
    {
        Car vehicle = new(VehicleTypes.Car, "EV0001", CarTypes.Sedan, 125, new ElectricEngine(408, 100.0m), 4, "White");

        StringBuilder expected = new();

        expected.AppendLine("Licence: EV0001, Type: Car");
        expected.AppendLine("Class: Sedan, Max speed: 125 km/h");
        expected.AppendLine("Color: White, Wheels: 4");
        expected.AppendLine("Engine: Electric, 100.0 kWh, 408HP");

        Assert.Equal(expected.ToString(), vehicle.FullDescription());
    }

    [Fact]
    public void Vehicle_CompareEquality()
    {
        Vehicle vehicle1 = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White");
        Vehicle vehicle2 = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White");

        Assert.Equal(vehicle1, vehicle2);
    }
}
