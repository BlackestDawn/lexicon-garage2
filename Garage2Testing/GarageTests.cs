using Garage2.Errors;
using Garage2.Models;
using Garage2.Models.Collections;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2Testing;

public class GarageTests
{
    [Fact]
    public void InitializeGarage_WithData()
    {
        Vehicle vehicle = new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White");
        Vehicle[] expected = [ new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White") ];

        Garage<Vehicle> garage = new(10, [vehicle]);

        Assert.Equal(expected, [.. garage]);
        Assert.Equal(1, garage.Length);
        Assert.Equal(1, garage.Count());
        Assert.Equal(10, garage.Capacity);
    }

    [Fact]
    public void InitializeGarage_WithTooMuchData_ThrowsDatasetTooLargeException()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), "Orange"),
        ];

        var ex = Assert.Throws<DatasetTooLargeException>(() => new Garage<Vehicle>(1, vehicles));
        Assert.Equal("Dataset too large: 3. Max size: 1", ex.Message);
    }

    [Fact]
    public void RemovingVehicle_ReducesAmounts()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), "Orange"),
        ];
        Vehicle[] expected = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), "Orange"),
        ];

        Garage<Vehicle> garage = new(5, vehicles);

        garage.Remove("BIKE42");
        var result = garage.ToArray();

        Assert.Equal(expected, result);
        Assert.Equal(2, garage.Length);
        Assert.Equal(0, garage.TypesCount[VehicleTypes.Motorcycle]);
        Assert.Equal(1, garage.TypesCount[VehicleTypes.Car]);
        Assert.Equal(1, garage.TypesCount[VehicleTypes.Bus]);
    }

    [Fact]
    public void Garage_RemovingNonExistentVehicle_ThrowsArgumentException()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), "Blue"),
            new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), "Orange"),
        ];
        Garage<Vehicle> garage = new(5, vehicles);

        Assert.Throws<ArgumentException>(() => garage.Remove("NOTHERE"));
    }

    [Fact]
    public void Garage_AddingWhenFull_ThrowsArgumentException()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), "Blue"),
        ];
        Garage<Vehicle> garage = new(2, vehicles);

        Assert.Throws<ArgumentException>(() => garage.Add(new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), "Orange")));
    }

    [Fact]
    public void Garage_RemovingWhenEmpty_ThrowsArgumentException()
    {
        Garage<Vehicle> garage = new(5);

        Assert.Throws<ArgumentException>(() => garage.Remove("BIKE42"));
    }

    [Fact]
    public void Garage_Adding_IncreasesAmountByType()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Car, "XYZ789", new FuelEngine(320, 3.0m, FuelTypes.Diesel), "Black"),
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
        ];
        Garage<Vehicle> garage = new(5, vehicles);

        Assert.Equal(3, garage.TypesCount[VehicleTypes.Car]);
    }

    [Fact]
    public void Garage_DeacreasingCapacityBelowLength_ThrowsArgumentException()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Car, "XYZ789", new FuelEngine(320, 3.0m, FuelTypes.Diesel), "Black"),
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
        ];
        Garage<Vehicle> garage = new(5, vehicles);

        Assert.Throws<ArgumentException>(() => garage.Resize(1));
    }

    [Fact]
    public void Garage_IEnumerableToArray_Converion()
    {
        Vehicle[] vehicles = [
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
            new(VehicleTypes.Car, "XYZ789", new FuelEngine(320, 3.0m, FuelTypes.Diesel), "Black"),
            new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
        ];
        Garage<Vehicle> garage = new(5, vehicles);

        var result = garage.ToArray();

        Assert.Equal(vehicles, result);
    }
}
