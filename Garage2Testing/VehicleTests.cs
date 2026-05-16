
using System.Text;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2Testing;

public class VehicleTests
{
    [Fact]
    public void Checking_MinimalDescription()
    {
        Vehicle vehicle = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White");

        string expected = "Licence: EV0001, Type: Car";

        Assert.Equal(expected, vehicle.MinimalDescription());
    }

    [Fact]
    public void Checking_Vehicle_FullDescription()
    {
        Vehicle vehicle = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White");

        StringBuilder expected = new();

        expected.AppendLine("Licence: EV0001, Type: Car");
        expected.AppendLine("Color: White, Wheels: 4");
        expected.AppendLine("Engine: Electric, 100.0 kWh, 408HP");

        Assert.Equal(expected.ToString(), vehicle.FullDescription());
    }

    [Fact]
    public void Checking_Car_Overrided_FullDescription()
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
    public void Checking_Equality()
    {
        Vehicle vehicle1 = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White");
        Vehicle vehicle2 = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White");

        Assert.Equal(vehicle1, vehicle2);
    }
}
