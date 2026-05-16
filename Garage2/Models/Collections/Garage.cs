using System.Collections;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2.Models.Collections;

public class Garage
{
    private readonly int _maxSpace;
    public int MaxSpace
    {
        get => _maxSpace;
    }
    private int _usedSpace = 0;
    public int UsedSpace
    {
        get => _usedSpace;
    }
    private readonly Vehicle[] _vehicles;
    private readonly Hashtable _amountByType;
    public Hashtable AmountsByVehicleType
    {
        get => _amountByType;
    }

    public Garage(int maxSpace)
    {
        _maxSpace = maxSpace;
        _vehicles = new Vehicle[maxSpace];
        _amountByType = new Hashtable();
        foreach (var type in Enum.GetValues<VehicleTypes>())
        {
            _amountByType.Add(type, 0);
        }
    }

    public Hashtable GetStatus()
    {
        return new Hashtable
        {
          { "total", _maxSpace },
          { "used",  _usedSpace },
          { "types", _amountByType }
        };
    }

    public Vehicle[] GetAllVehicles()
    {
        Vehicle[] parkedVehicles = new Vehicle[_usedSpace];
        int index = 0;
        foreach (var vehicle in _vehicles)
        {
            if (vehicle != null)
            {
                parkedVehicles[index] = vehicle;
                index++;
            }
        }
        return parkedVehicles;
    }

    public string[] GetAllLicenceNumbers()
    {
        string[] licenceNumbers = new string[_usedSpace];
        int index = 0;
        foreach (var vehicle in _vehicles)
        {
            if (vehicle != null)
            {
                licenceNumbers[index] = vehicle.LicenceNumber;
                index++;
            }
        }
        return licenceNumbers;
    }

    public void ListVehicleAmountByType()
    {
        throw new NotImplementedException();
    }

    public bool CheckIfLicencePresent(string licence)
    {
        for (int i = 0; i < _maxSpace; i++)
        {
            if (_vehicles[i] != null && _vehicles[i].LicenceNumber.ToLower() == licence.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (_usedSpace == _maxSpace)
        {
            throw new ArgumentException("Space is full");
        }
        for (int i = 0; i < _maxSpace; i++)
        {
            if (_vehicles[i] == null)
            {
                var vehicleType = vehicle.VehicleType;
                _amountByType[vehicleType] = (int)_amountByType[vehicleType] + 1;
                _vehicles[i] = vehicle;
                _usedSpace++;
                return;
            }
        }
    }

    public void RemoveVehicle(string licenceNumber)
    {
        if (_usedSpace == 0)
        {
            throw new ArgumentException("Space is empty");
        }
        for (int i = 0; i < _maxSpace; i++)
        {
            if (_vehicles[i] != null && _vehicles[i].LicenceNumber.ToLower() == licenceNumber.ToLower())
            {
                var vehicleType = _vehicles[i].VehicleType;
                _amountByType[vehicleType] = (int)_amountByType[vehicleType] - 1;
                _vehicles[i] = null;
                _usedSpace--;
                return;
            }
        }
        throw new ArgumentException($"Vehicle with number {licenceNumber} not found");
    }

    public void BulkLoadVehicles(Vehicle[] vehicles)
    {
        try {
            foreach (var vehicle in vehicles)
            {
                AddVehicle(vehicle);
            }
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Could not load all vehicles: {ex.Message}");
        }
    }

    public Vehicle[] FindVehicles(Func<Vehicle, bool> predicate)
    {
        return [.. _vehicles.Where(v => v != null && predicate(v))];
    }
}
