using System.ComponentModel;

namespace Garage2.Models.Enums;

public enum MainMenuOptions
{
    [Description("View vehicles")]      List,
    [Description("Park a vehicle")]     Add,
    [Description("Release a vehicle")]  Remove,
    [Description("Search for vehicle")] Search,
    [Description("Quit")]               Quit,
}
