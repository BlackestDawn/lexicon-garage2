using Garage2.Models.Collections;
using Garage2.Models.Data;
using Garage2.Models.Enums;
using Garage2.Models.Interfaces;
using Garage2.Models.Vehicles;
using Spectre.Console;

namespace Garage2.Services;

public class ManagementApp
{
    private readonly Garage<Vehicle> _garage;
    private readonly IUI _ui;

    public ManagementApp()
    {
        _garage = new Garage<Vehicle>(20, TestData.testVehicles);
        _ui = new ConsoleUI(UsageStats);
    }

    public void RunApp()
    {
        MainMenuOptions menuChoice;

        do {
            menuChoice = _ui.MainMenuWindow();

            try {
                switch (menuChoice)
                {
                    case MainMenuOptions.Quit:
                        AnsiConsole.MarkupLine("Exiting");
                        break;
                    case MainMenuOptions.List:
                        Vehicle[] vehicles = _garage.Vehicles;
                        if (vehicles.Length > 0)
                        {
                            _ui.VehicleDetailsWindow(_ui.VehicleListSelectionWindow(vehicles));
                        }
                        else
                        {
                            _ui.Message("Nothing to view, no vehicles parked.", MessageTypes.Warning);
                        }
                        _ui.PauseDisplay();
                        _ui.ResetMenuPath();
                        break;
                    case MainMenuOptions.Add:
                        if (_garage.Length < _garage.Capacity)
                        {
                            Vehicle vehicle = _ui.AddVehicleWindow();
                            if (!_garage.Vehicles.Any(v => v.LicenceNumber == vehicle.LicenceNumber))
                            {
                                _garage.Add(vehicle);
                                _ui.Message($"Vehicle '{vehicle.MinimalDescription()}' added.", MessageTypes.Success);
                            }
                            else
                            {
                                _ui.Message($"Vehicle with licence {vehicle.LicenceNumber} already parked", MessageTypes.Warning);
                            }
                        }
                        else
                        {
                            _ui.Message("Can't add vehicle, no more space left.", MessageTypes.Warning);
                        }
                        _ui.ResetMenuPath();
                        break;
                    case MainMenuOptions.Remove:
                        string[] licenses = _garage.Vehicles.Select(v => v.LicenceNumber).ToArray();
                        if (licenses.Length > 0)
                        {
                            string licence = _ui.RemoveVehicleWindow(licenses);
                            _garage.Remove(licence);
                            _ui.Message($"Vechile with licence number '{licence}' removed.", MessageTypes.Warning);
                        }
                        else
                        {
                            _ui.Message("Nothing to remove, no vehicles parked.", MessageTypes.Warning);
                        }
                        _ui.ResetMenuPath();
                        break;
                    case MainMenuOptions.Search:
                        var searchParams = _ui.SearchInputWindow();
                        if (searchParams != null)
                        {
                            Vehicle[] result = _garage.Vehicles.Where(searchParams).ToArray();
                            if (result.Length > 0)
                            {
                                _ui.SearchResultWindow(result);
                                _ui.PauseDisplay();
                            }
                            else
                            {
                                _ui.Message("No vehicles found", MessageTypes.Warning);
                            }
                        }
                        else
                        {
                            _ui.Message("No search terms specified", MessageTypes.Warning);
                        }
                        _ui.ResetMenuPath();
                        break;
                    default:
                        _ui.Message($"Menu option does not exist or is not implemented yet: {menuChoice}", MessageTypes.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                _ui.Message($"Something went wrong{Environment.NewLine}{ex.Message}", MessageTypes.Error);
                _ui.ResetMenuPath();
            }
        } while (menuChoice != MainMenuOptions.Quit);
    }

    private Dictionary<string, int> UsageStats()
    {
        Dictionary<string, int> stats = [];
        stats["max"] = _garage.Capacity;
        stats["used"] = _garage.Length;
        foreach (var item in _garage.TypesCount)
        {
            stats[item.Key.ToString()] = item.Value;
        }

        return stats;
    }
}
