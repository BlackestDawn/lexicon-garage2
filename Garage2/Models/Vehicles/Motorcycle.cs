using System.Text;
using Garage2.Extensions;
using Garage2.Models.Enums;
using Garage2.Models.Interfaces;

namespace Garage2.Models.Vehicles;

public class Motorcycle(VehicleTypes vehicleType, string licenceNumber, int maxSpeed, IEngine engine, int numWheels, string color)
    : WheeledVehicle(vehicleType, licenceNumber, engine, numWheels, color)
{
    private readonly int _maxSpeed = maxSpeed;
    public int MaxSpeed
    {
        get => _maxSpeed;
    }

  public override string FullDescription()
    {
        StringBuilder sb = new(base.FullDescription());

        sb.InsertLine(1, $"Top speed: {_maxSpeed}");

        return sb.ToString();
    }
}
