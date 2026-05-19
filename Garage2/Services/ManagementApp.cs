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
        _ui = new ConsoleUI(_garage);
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
                        if (_garage.Length > 0)
                        {
                            _ui.VehicleDetailsWindow(_ui.VehicleListSelectionWindow(_garage));
                        }
                        else
                        {
                            _ui.Message("Nothing to view, no vehicles parked.", MessageTypes.Warning);
                        }
                        break;
                    case MainMenuOptions.Add:
                        if (_garage.Length < _garage.Capacity)
                        {
                            var vehicle = _ui.AddVehicleWindow(_garage.Select(v => v.LicenceNumber));
                            if (vehicle != null)
                            {
                                _garage.Add(vehicle);
                                _ui.Message($"Vehicle '{vehicle.MinimalDescription()}' added.", MessageTypes.Success);
                            }
                        }
                        else
                        {
                            _ui.Message("Can't add vehicle, no more space left.", MessageTypes.Warning);
                        }
                        break;
                    case MainMenuOptions.Remove:
                        if (_garage.Length > 0)
                        {
                            string licence = _ui.RemoveVehicleWindow(_garage.Select(v => v.LicenceNumber));
                            _garage.Remove(licence);
                            _ui.Message($"Vehicle with licence number '{licence}' removed.", MessageTypes.Warning);
                        }
                        else
                        {
                            _ui.Message("Nothing to remove, no vehicles parked.", MessageTypes.Warning);
                        }
                        break;
                    case MainMenuOptions.Search:
                        var searchParams = _ui.SearchInputWindow();
                        if (searchParams != null)
                        {
                            var result = _garage.Where(searchParams);
                            if (result.Any())
                            {
                                _ui.SearchResultWindow(result);
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
                        break;
                    default:
                        _ui.Message($"Menu option does not exist or is not implemented yet: {menuChoice}", MessageTypes.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                _ui.Message($"Something went wrong{Environment.NewLine}{ex.Message}", MessageTypes.Error);
            }
        } while (menuChoice != MainMenuOptions.Quit);
    }
}
