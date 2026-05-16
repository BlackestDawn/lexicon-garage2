using System.Collections;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2.Models.Collections;

public class Garage<T> : IEnumerable where T: Vehicle
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
    private readonly Vehicle?[] _vehicles;
    public Vehicle[] Vehicles
    {
        get
        {
            return _usedSpace > 0 ? [.. _vehicles.Where(v => v != null)] : [];
        }
    }
    private readonly Dictionary<VehicleTypes, int> _amountByType;
    public Dictionary<VehicleTypes, int> TypesCount
    {
        get => _amountByType;
    }

    public Garage(int maxSpace)
    {
        _maxSpace = maxSpace;
        _vehicles = new Vehicle?[maxSpace];
        _amountByType = [];
        foreach (var type in Enum.GetValues<VehicleTypes>())
        {
            _amountByType[type] = 0;
        }
    }

    public Garage(int maxSpace, Vehicle[] vehicles) : this(maxSpace)
    {
        int newCount = vehicles.Count();
        if (newCount > _maxSpace)
        {
            throw new ArgumentOutOfRangeException("Vehicle count is higher than capacity");
        }
        for (int i = 0; i < newCount; i++)
        {
            _vehicles[i] = vehicles[i];
            _amountByType[vehicles[i].VehicleType]++;
        }
        _usedSpace = newCount;
    }

    public void Add(Vehicle vehicle)
    {
        if (_usedSpace >= _maxSpace)
        {
            throw new ArgumentException("Space is full");
        }
        for (int i = 0; i < _maxSpace; i++)
        {
            if (_vehicles[i] == null)
            {
                _amountByType[vehicle.VehicleType]++;
                _vehicles[i] = vehicle;
                _usedSpace++;
                return;
            }
        }
    }

    public void Remove(string licenceNumber)
    {
        if (_usedSpace == 0)
        {
            throw new ArgumentException("Space is empty");
        }
        for (int i = 0; i < _maxSpace; i++)
        {
            if (_vehicles[i] != null && _vehicles[i].LicenceNumber == licenceNumber)
            {
                _amountByType[_vehicles[i].VehicleType]--;
                _vehicles[i] = null;
                _usedSpace--;
                return;
            }
        }
        throw new ArgumentException($"Vehicle with number {licenceNumber} not found");
    }

    public void Remove(Vehicle vehicle)
    {
        Remove(vehicle.LicenceNumber);
    }

    public Vehicle[] Find(Func<Vehicle, bool> predicate)
    {
        return [.. _vehicles.Where(v => v != null && predicate(v))];
    }

    public IEnumerator GetEnumerator() => Vehicles.GetEnumerator();
    public int Count() => _usedSpace;
}
