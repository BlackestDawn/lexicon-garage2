using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2.Models.Data;

public static class TestData
{
    public static Vehicle[] testVehicles =
    [
        new Car(VehicleTypes.Car, "ABC123", CarTypes.Sedan, 150, new FuelEngine(150, 2.0m, FuelTypes.Gasoline), 4, "Red"),
        new Car(VehicleTypes.Car, "XYZ789", CarTypes.Pickup, 140, new FuelEngine(320, 3.0m, FuelTypes.Diesel), 4, "Black"),
        new Car(VehicleTypes.Car, "EV0001", CarTypes.Coupe, 135, new ElectricEngine(408, 100.0m), 4, "White"),
        new Motorcycle(VehicleTypes.Motorcycle, "BIKE42", 170, new FuelEngine(85, 0.6m, FuelTypes.Gasoline), 2, "Blue"),
        new Bus(VehicleTypes.Bus, "TRK999", 20, new FuelEngine(500, 12.7m, FuelTypes.Diesel), 18, "Orange"),
        new Bus(VehicleTypes.Bus, "EV0042", 50, new ElectricEngine(670, 200.0m), 18, "Silver"),
        new Airplane(VehicleTypes.Airplane, "AIR001", 1, new FuelEngine(260, 5.2m, FuelTypes.Avgas), 3, "Yellow"),
        new Airplane(VehicleTypes.Airplane, "JET007", 2, new FuelEngine(90000, 0.0m, FuelTypes.JetAA1), 3, "White"),
        new Car(VehicleTypes.Car, "BIO555", CarTypes.Sport, 160, new FuelEngine(110, 1.6m, FuelTypes.Biofuel), 4, "Green"),
        new Boat(VehicleTypes.Boat, "EV0099", 2, new ElectricEngine(204, 58.0m), "Gray"),
    ];
}
