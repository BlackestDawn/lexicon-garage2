using System.Text;
using Garage2.Extensions;
using Garage2.Models.Interfaces;

namespace Garage2.Models.Vehicles;

public class Car(VehicleTypes vehicleType, string licenceNumber, CarTypes carType, int maxSpeed, IEngine engine, int numWheels, string color)
    : Vehicle(vehicleType, licenceNumber, engine, numWheels, color)
{
    private readonly CarTypes _carType = carType;
    private readonly int _maxSpeed = maxSpeed;

  public override string FullDescription()
    {
        StringBuilder sb = new(base.FullDescription());

        sb.InsertLine(1, $"Class: {_carType}, Max speed: {_maxSpeed} km/h");

        return sb.ToString();
    }
}
