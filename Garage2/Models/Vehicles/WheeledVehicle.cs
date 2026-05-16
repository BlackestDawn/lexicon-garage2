using System.Text;
using Garage2.Extensions;
using Garage2.Models.Enums;
using Garage2.Models.Interfaces;

namespace Garage2.Models.Vehicles;

public class WheeledVehicle(VehicleTypes vehicleType, string licenceNumber, IEngine engine, int numWheels, string color)
    : Vehicle(vehicleType, licenceNumber, engine, color)
{
    private readonly int _numWheels = numWheels;
    public int WheelCount
    {
        get => _numWheels;
    }

    public override string FullDescription()
    {
        StringBuilder sb = new(base.FullDescription());

        sb.AppendToLine(1, $", Wheels: {_numWheels}");

        return sb.ToString();
    }
}
