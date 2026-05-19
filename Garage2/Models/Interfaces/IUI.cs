using Garage2.Models.Enums;
using Garage2.Models.Vehicles;

namespace Garage2.Models.Interfaces;

public interface IUI
{
    public MainMenuOptions MainMenuWindow();
    public Vehicle VehicleListSelectionWindow(IEnumerable<Vehicle> vehicles);
    public string RemoveVehicleWindow(IEnumerable<string> licenceNumbers);
    public Vehicle AddVehicleWindow();
    public void VehicleDetailsWindow(Vehicle vehicle);
    public void Message(string content, MessageTypes type = MessageTypes.Standard);
    public Func<Vehicle, bool>? SearchInputWindow();
    public void SearchResultWindow(IEnumerable<Vehicle> vehicles);
}
