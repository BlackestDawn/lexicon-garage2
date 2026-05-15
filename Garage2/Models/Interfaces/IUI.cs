using Garage2.Models.Vehicles;

namespace Garage2.Models.Interfaces;

public interface IUI
{
    public MainMenuOptions MainMenuWindow();
    public Vehicle VehicleListSelectionWindow(Vehicle[] vehicles);
    public string RemoveVehicleWindow(string[] licenceNumbers);
    public Vehicle AddVehicleWindow();
    public void PauseDisplay(string message = "Press any key to continue");
    public void ResetMenuPath();
    public void VehicleDetailsWindow(Vehicle vehicle);
    public void ErrorMessage(string message);
    public void WarningMessage(string message);
    public void SuccessMessage(string message);
    public Func<Vehicle, bool>? SearchInputWindow();
    public void SearchResultWindow(Vehicle[] vehicles);
}
