using System.Collections;
using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2.Models.Collections;

public class Garage<T> : IEnumerable<T> where T: Vehicle
{
    private int _capacity;
    public int Capacity
    {
        get => _capacity;
    }
    private int _length = 0;
    public int Length
    {
        get => _length;
    }
    private T?[] _vehicles;
    private readonly Dictionary<VehicleTypes, int> _amountByType;
    public Dictionary<VehicleTypes, int> TypesCount
    {
        get => _amountByType;
    }

    public Garage(int capacity)
    {
        _capacity = capacity;
        _vehicles = new T?[capacity];
        _amountByType = [];
        foreach (var type in Enum.GetValues<VehicleTypes>())
        {
            _amountByType[type] = 0;
        }
    }

    public Garage(int capacity, T[] vehicles) : this(capacity)
    {
        int newCount = vehicles.Count();
        if (newCount > _capacity)
        {
            throw new ArgumentOutOfRangeException("Vehicle count is higher than capacity");
        }
        for (int i = 0; i < newCount; i++)
        {
            _vehicles[i] = vehicles[i];
            _amountByType[vehicles[i].VehicleType]++;
        }
        _length = newCount;
    }

    public void Resize(int capacity)
    {
        if (capacity < _length)
        {
            throw new ArgumentException("Cannot resize below current length");
        }

        Array.Resize(ref _vehicles, capacity);
        _capacity = capacity;
    }

    public void Add(T vehicle)
    {
        if (_length >= _capacity)
        {
            throw new ArgumentException("Space is full");
        }
        for (int i = 0; i < _capacity; i++)
        {
            if (_vehicles[i] == null)
            {
                _amountByType[vehicle.VehicleType]++;
                _vehicles[i] = vehicle;
                _length++;
                return;
            }
        }
    }

    public void Remove(string licenceNumber)
    {
        if (_length == 0)
        {
            throw new ArgumentException("Space is empty");
        }
        for (int i = 0; i < _capacity; i++)
        {
            if (_vehicles[i] != null && _vehicles[i].LicenceNumber == licenceNumber)
            {
                _amountByType[_vehicles[i].VehicleType]--;
                _vehicles[i] = null;
                _length--;
                return;
            }
        }
        throw new ArgumentException($"Vehicle with number {licenceNumber} not found");
    }

    public void Remove(T vehicle)
    {
        Remove(vehicle.LicenceNumber);
    }

    public T[] Find(Func<T, bool> predicate)
    {
        return [.. _vehicles.Where(v => v != null && predicate(v))];
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _vehicles)
        {
            if (item != null)
            {
                yield return item;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count() => _length;
}
