using System.Text;
using Garage2.Models.Enums;
using Garage2.Models.Interfaces;

namespace Garage2.Models.Vehicles;

public class Vehicle(VehicleTypes vehicleType, string licenceNumber, IEngine engine, string color) : IPrintable
{
    private readonly VehicleTypes _vehicleType = vehicleType;
    public VehicleTypes VehicleType
    {
        get => _vehicleType;
    }
    private readonly string _licenceNumber = licenceNumber.ToUpper();
    public string LicenceNumber
    {
        get => _licenceNumber;
    }
    private readonly IEngine _engine = engine;
    public IEngine Engine
    {
        get => _engine;
    }
    private readonly string _color = color;
    public string Color
    {
        get => _color;
    }

    public override string ToString()
    {
        return $"Licence number: {_licenceNumber}\nEngine: {_engine.Description}\nColor: {_color}";
    }

    public override bool Equals(object? obj)
        {
            if (obj is not Vehicle other) return false;
            return _licenceNumber == other.LicenceNumber;
        }

    public override int GetHashCode() => _licenceNumber.GetHashCode();

    public string MinimalDescription()
    {
        return $"Licence: {_licenceNumber}, Type: {_vehicleType}";
    }

    public virtual string FullDescription()
    {
        StringBuilder sb = new();

        sb.AppendLine(MinimalDescription());
        sb.AppendLine($"Color: {_color}");
        sb.AppendLine($"Engine: {_engine.Description}");

        return sb.ToString();
    }
}
