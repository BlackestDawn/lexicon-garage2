using System.Text;
using Garage2.Extensions;
using Garage2.Models.Enums;
using Garage2.Models.Interfaces;

namespace Garage2.Models.Vehicles;

public class Airplane(VehicleTypes vehicleType, string licenceNumber, int engineCount, IEngine engine, int numWheels, string color)
    : Vehicle(vehicleType, licenceNumber, engine, numWheels, color)
{
    private readonly int _engineCount = engineCount;

  public override string FullDescription()
    {
        StringBuilder sb = new(base.FullDescription());

        sb.InsertLine(1, $"Engine count: {_engineCount}");

        return sb.ToString();
    }
}
