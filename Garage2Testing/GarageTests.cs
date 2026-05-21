using Garage2.Errors;
using Garage2.Models;
using Garage2.Models.Collections;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2Testing;

public class GarageTests
{
    private readonly Vehicle[] _smallVehicleSet =
    [
        new(VehicleTypes.Car, "EV0001", new ElectricEngine(408, 100.0m), "White"),
        new(VehicleTypes.Motorcycle, "BIKE42", new FuelEngine(85, 0.6m, FuelTypes.Gasoline), "Blue"),
        new(VehicleTypes.Bus, "TRK999", new FuelEngine(500, 12.7m, FuelTypes.Diesel), "Orange"),
    ];
    private readonly Garage<Vehicle> _garage;

    public GarageTests()
    {
        _garage = new(5, _smallVehicleSet);
    }

    [Fact]
    public void InitializeGarage_WithData()
    {
        Assert.Equal(_smallVehicleSet, _garage);
        Assert.Equal(3, _garage.Length);
        Assert.Equal(3, _garage.Count());
        Assert.Equal(5, _garage.Capacity);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Car]);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Motorcycle]);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Bus]);
    }

    [Fact]
    public void InitializeGarage_WithTooMuchData_ThrowsDatasetTooLargeException()
    {
        var ex = Assert.Throws<DatasetTooLargeException>(() => new Garage<Vehicle>(1, _smallVehicleSet));
        Assert.Equal("Dataset too large: 3. Max size: 1", ex.Message);
    }

    [Fact]
    public void RemovingVehicle_ReducesAmounts()
    {
        Vehicle[] expected = [ _smallVehicleSet[0], _smallVehicleSet[2]];
        _garage.Remove("BIKE42");

        Assert.Equal(expected, _garage);
        Assert.Equal(2, _garage.Length);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Car]);
        Assert.Equal(0, _garage.TypesCount[VehicleTypes.Motorcycle]);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Bus]);
    }

    [Fact]
    public void RemovingNonExistentVehicle_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _garage.Remove("NOTHERE"));
        Assert.Equal("Vehicle with number NOTHERE not found", ex.Message);
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
    public void AddingVehicle_IncreasesAmounts()
    {
        _garage.Add(new(VehicleTypes.Car, "XYZ789", new FuelEngine(320, 3.0m, FuelTypes.Diesel), "Black"));
        _garage.Add(new(VehicleTypes.Airplane, "AIR001", new FuelEngine(260, 5.2m, FuelTypes.Avgas), "Yellow"));

        Assert.Equal(5, _garage.Length);
        Assert.Equal(2, _garage.TypesCount[VehicleTypes.Car]);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Motorcycle]);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Bus]);
        Assert.Equal(1, _garage.TypesCount[VehicleTypes.Airplane]);
    }

    [Fact]
    public void Garage_DeacreasingCapacityBelowLength_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _garage.Resize(1));
    }

    [Fact]
    public void Garage_IEnumerableToArray_Converion()
    {
        Assert.Equal(_smallVehicleSet, _garage.ToArray());
    }
}
