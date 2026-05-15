using System.Text;
using Garage2.Extensions;
using Garage2.Models.Interfaces;

namespace Garage2.Models.Vehicles;

public class Bus(VehicleTypes vehicleType, string licenceNumber, int passangerCapacity, IEngine engine, int numWheels, string color)
    : Vehicle(vehicleType, licenceNumber, engine, numWheels, color)
{
    private readonly int _passengerCapacity = passangerCapacity;

  public override string FullDescription()
    {
        StringBuilder sb = new(base.FullDescription());

        sb.InsertLine(1, $"Passenger capacity: {_passengerCapacity}");

        return sb.ToString();
    }
}
