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
                            _ui.WarningMessage("Nothing to view, no vehicles parked.");
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
                                _ui.SuccessMessage($"Vehicle '{vehicle.MinimalDescription()}' added.");
                            }
                            else
                            {
                                _ui.WarningMessage($"Vehicle with licence {vehicle.LicenceNumber} already parked");
                            }
                        }
                        else
                        {
                            _ui.WarningMessage("Can't add vehicle, no more space left.");
                        }
                        _ui.ResetMenuPath();
                        break;
                    case MainMenuOptions.Remove:
                        string[] licenses = _garage.Vehicles.Select(v => v.LicenceNumber).ToArray();
                        if (licenses.Length > 0)
                        {
                            string licence = _ui.RemoveVehicleWindow(licenses);
                            _garage.Remove(licence);
                            _ui.SuccessMessage($"Vechile with licence number '{licence}' removed.");
                        }
                        else
                        {
                            _ui.WarningMessage("Nothing to remove, no vehicles parked.");
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
                                _ui.WarningMessage("No vehicles found");
                            }
                        }
                        else
                        {
                            _ui.WarningMessage("No search terms specified");
                        }
                        _ui.ResetMenuPath();
                        break;
                    default:
                        _ui.ErrorMessage($"Menu option does not exist or is not implemented yet: {menuChoice}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _ui.ErrorMessage(ex.Message);
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
