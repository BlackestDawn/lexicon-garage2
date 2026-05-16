using Garage2.Models;
using Garage2.Models.Collections;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2Testing;

public class GarageTests
{
    [Fact]
    public void AddingVehicle()
    {
        Vehicle vehicle = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White");
        Vehicle[] expected = [ new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White") ];
        Garage garage = new(3);

        garage.AddVehicle(vehicle);
        var result = garage.GetAllVehicles();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void AddingVehicles_BeyondCapacity_ThrowsArgumentException()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Vehicle[] expected = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Garage garage = new(1);

        Assert.Throws<ArgumentException>(() => garage.BulkLoadVehicles(vehicles));
    }

    [Fact]
    public void BulkAddingVehicles()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Vehicle[] expected = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Garage garage = new(5);

        garage.BulkLoadVehicles(vehicles);
        var result = garage.GetAllVehicles();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void RemovingVehicles()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Vehicle[] expected = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Garage garage = new(5);

        garage.BulkLoadVehicles(vehicles);
        garage.RemoveVehicle("BIKE42");
        var result = garage.GetAllVehicles();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void RemovingNonExistentVehicle_ThrowsArgumentException()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        ];
        Garage garage = new(5);

        garage.BulkLoadVehicles(vehicles);
        Assert.Throws<ArgumentException>(() => garage.RemoveVehicle("NOTHERE"));
    }

    [Fact]
    public void RemovingVehicle_WhenEmpty_ThrowsArgumentException()
    {
        Garage garage = new(5);

        Assert.Throws<ArgumentException>(() => garage.RemoveVehicle("BIKE42"));
    }

    [Fact]
    public void AddingCars_Increases_AmountByType()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Car, "XYZ789", new FuelEngine(320, 3.0m, FuelTypes.Diesel), 4, "Black"),
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
        ];
        Garage garage = new(5);
        garage.BulkLoadVehicles(vehicles);

        Assert.Equal(3, (int)garage.AmountsByVehicleType[VehicleTypes.Car]);
    }

    [Fact]
    public void RemovingCars_Decreases_AmountByType()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
            new(VehicleTypes.Car, "XYZ789", new FuelEngine(320, 3.0m, FuelTypes.Diesel), 4, "Black"),
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), 4, "White"),
        ];
        Garage garage = new(5);
        garage.BulkLoadVehicles(vehicles);
        garage.RemoveVehicle("XYZ789");

        Assert.Equal(2, (int)garage.AmountsByVehicleType[VehicleTypes.Car]);
    }
}
