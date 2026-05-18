using System.Collections;
using System.Text;
using Garage2.Extensions;
using Garage2.Helpers;
using Garage2.Models;
using Garage2.Models.Enums;
using Garage2.Models.Interfaces;
using Garage2.Models.Vehicles;
using Spectre.Console;

namespace Garage2.Services;

public class ConsoleUI: IUI
{
    private readonly Func<Dictionary<string, int>> _usageStatus;
    private readonly Stack _menuPath = new(5);
    private readonly Color[] _typesColor =
    [
        Color.Magenta,
        Color.LightGreen,
        Color.Cyan,
        Color.DarkBlue,
        Color.LightPink4
    ];

    public ConsoleUI(Func<Dictionary<string, int>> getStatus)
    {
        _usageStatus = getStatus;
        _menuPath.Push("Main Menu");
    }

    private void RenderHeader()
    {
        StringBuilder menuPathSB = new();
        int depthCounter = _menuPath.Count;
        foreach (string item in _menuPath)
        {
            menuPathSB.PrependLine($"{new String(' ', 2 * depthCounter)}{item}");
            depthCounter--;
        }

        Panel menuPathPanel = new Panel(menuPathSB.ToString())
            .Header("  Menu path")
            .Expand()
            .NoBorder()
            .Padding(0, 2);

        var currentStatus = _usageStatus();

        Panel usagePanel = new Panel(
            new BreakdownChart()
                .Width(20)
                .AddItem("Used:", currentStatus["used"], Color.Red3)
                .AddItem("Free:", currentStatus["max"] - currentStatus["used"], Color.LightYellow3)
            )
            .Header("Space usage")
            .NoBorder()
            .Padding(2, 1);

        BreakdownChartItem[] typesBreakdown = new BreakdownChartItem[Enum.GetNames<VehicleTypes>().Length];

        foreach (var item in Enum.GetValues<VehicleTypes>())
        {
            string key = item.ToString();
            int index = (int)item;
            typesBreakdown[index] = new BreakdownChartItem($"{key}", _usageStatus()[key], _typesColor[index]);
        }

        Panel typesPanel = new Panel(
            new BreakdownChart()
                .Width(30)
                .AddItems(typesBreakdown)
            )
            .Header("Usage by types")
            .NoBorder()
            .Padding(2, 1);

        Grid mainGrid = new Grid()
            .AddColumns(3)
            .AddRow(
                Align.Left(menuPathPanel),
                Align.Center(usagePanel),
                Align.Right(typesPanel)
            );

        Panel mainPanel = new Panel(mainGrid)
            .Header("[yellow]===[/] [cadetBlue]Garage Management[/] [yellow]===[/]", Justify.Center)
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .Padding(4, 2)
            .Expand();

        AnsiConsole.Clear();
        AnsiConsole.Write(mainPanel);
    }

    public void PauseDisplay(string message = "Press any key to continue")
    {
        AnsiConsole.MarkupLine($"\n[gray]{message}[/]");
        Console.ReadKey(intercept: true);
    }

    public void ResetMenuPath()
    {
        _menuPath.Clear();
        _menuPath.Push("Main Menu");
    }

    public MainMenuOptions MainMenuWindow()
    {
        RenderHeader();
        return AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOptions>()
                .UseConverter(EnumHelpers.GetDescription)
                .AddChoices(Enum.GetValues<MainMenuOptions>())
        );
    }

    public Vehicle VehicleListSelectionWindow(Vehicle[] vehicles)
    {
        _menuPath.Push("Vehicle list");
        RenderHeader();
        AnsiConsole.Write(new Text("All parked vehicles:\n"));
        return AnsiConsole.Prompt(
            new SelectionPrompt<Vehicle>()
                .UseConverter(v => v.MinimalDescription())
                .AddChoices(vehicles)
        );
    }

    public string RemoveVehicleWindow(string[] licenceNumbers)
    {
        _menuPath.Push("Releasing vehicle");
        RenderHeader();
        string licenceNumber = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select vehicle to release:")
                .AddChoices(licenceNumbers)
        );
        _menuPath.Pop();
        return licenceNumber;
    }

    public Vehicle AddVehicleWindow()
    {
        _menuPath.Push("Adding vehicle");
        RenderHeader();

        VehicleTypes vehicleType = AskForVehicleType();
        Vehicle newVehicle;

        switch (vehicleType)
        {
            case VehicleTypes.Car:
                newVehicle = new Car(
                    vehicleType,
                    AskForLicenceNumber(),
                    AskForCarType(),
                    AskForMaxSpeed(),
                    AskForEngine(),
                    AskForWheelCount(),
                    AskForColor()
                );
                break;
            case VehicleTypes.Bus:
                newVehicle = new Bus(
                    vehicleType,
                    AskForLicenceNumber(),
                    AskForPassengerCount(),
                    AskForEngine(),
                    AskForWheelCount(),
                    AskForColor()
                );
                break;
            case VehicleTypes.Motorcycle:
                newVehicle = new Motorcycle(
                    vehicleType,
                    AskForLicenceNumber(),
                    AskForMaxSpeed(),
                    AskForEngine(),
                    AskForWheelCount(),
                    AskForColor()
                );
                break;
            case VehicleTypes.Boat:
                newVehicle = new Boat(
                    vehicleType,
                    AskForLicenceNumber(),
                    AskForEngineCount(),
                    AskForEngine(),
                    AskForColor()
                );
                break;
            case VehicleTypes.Airplane:
                newVehicle = new Airplane(
                    vehicleType,
                    AskForLicenceNumber(),
                    AskForEngineCount(),
                    AskForEngine(),
                    AskForWheelCount(),
                    AskForColor()
                );
                break;
            default:
                throw new ArgumentException($"Unknown vehicle type: {vehicleType}");
        }

        _menuPath.Pop();
        return newVehicle;
    }

    private VehicleTypes AskForVehicleType()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<VehicleTypes>()
                .Title("Select vehicle type:")
                .AddChoices(Enum.GetValues<VehicleTypes>())
        );
    }

    private string AskForLicenceNumber()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter licence number:")
                .Validate(input => input.Length >= 6, "Must be at least 6 characters")
            );
    }

    private CarTypes AskForCarType()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<CarTypes>()
                .Title("Select car type:")
                .UseConverter(EnumHelpers.GetDescription)
                .AddChoices(Enum.GetValues<CarTypes>())
        );
    }

    private int AskForMaxSpeed()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter top speed of vehicle:")
                .Validate(input => input > 0, "Must be a positive number")
            );
    }

    private int AskForPassengerCount()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter passenger capacity:")
                .Validate(input => input > 0, "Must be a positive number")
            );
    }

    private int AskForEngineCount()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter number of engines:")
                .Validate(input => input > 1, "Must have at least 1 engine")
            );
    }

    private IEngine AskForEngine()
    {
        var engineSelection = AskForEngineType();

        if (engineSelection == "Fuel based")
        {
            return new FuelEngine(
                AskForEngineHP(),
                AskForEngineDisplacementVolume(),
                AskForEngineFuelType()
            );
        }
        else
        {
            return new ElectricEngine(
                AskForEngineHP(),
                AskForEngineBatteryCapacity()
            );
        }
    }

    private string AskForEngineType()
    {
        return AnsiConsole.Prompt<string>(
            new SelectionPrompt<string>()
                .Title("Select engine type")
                .AddChoices("Fuel based", "Electric")
            );
    }

    private int AskForEngineHP()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter horse power of engine:")
                .Validate(input => input > 0, "Must be a positive number")
            );
    }

    private FuelTypes AskForEngineFuelType()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<FuelTypes>()
                    .Title("Select engine's fuel type:")
                    .AddChoices(Enum.GetValues<FuelTypes>())
            );
    }

    private decimal AskForEngineDisplacementVolume()
    {
        return AnsiConsole.Prompt(
                new TextPrompt<decimal>("Enter engine's displacement volume in liters:")
                    .Validate(input => input > 0, "Must be a positive number")
            );
    }

    private decimal AskForEngineBatteryCapacity()
    {
        return AnsiConsole.Prompt(
                new TextPrompt<decimal>("Enter engine's battery capacity in kWh:")
                    .Validate(input => input > 0, "Must be a positive number")
            );
    }

    private int AskForWheelCount()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter number of wheels:")
                .Validate(input => input >= 0, "Cannot be a negative number")
        );
    }

    private string AskForColor()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter vehicle's color:")
        );
    }

    public void VehicleDetailsWindow(Vehicle vehicle)
    {
        _menuPath.Push($"{vehicle.VehicleType} details");
        RenderHeader();

        AnsiConsole.MarkupLine(vehicle.FullDescription());
    }

    public void ErrorMessage(string message)
    {
        AnsiConsole.MarkupLine($"[red]Something went wrong:{Environment.NewLine}{message}[/]");
        PauseDisplay();
    }

    public void SuccessMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{message}[/]");
        PauseDisplay();
    }

    public void WarningMessage(string message)
    {
        AnsiConsole.MarkupLine($"[yellow]{message}[/]");
        PauseDisplay();
    }

    public Func<Vehicle, bool>? SearchInputWindow()
    {
        _menuPath.Push("Search terms");
        RenderHeader();

        var fields = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Choose fields to search on:")
                .AddChoices([
                    "Licence number",
                    "Vehicle type",
                    "Color",
                    "Wheel count",
                    "Max effect (HP)"
                ])
            );

        var predicates = new List<Func<Vehicle, bool>>();

        if (fields.Contains("Licence number"))
        {
            string value = AskForLicenceNumber();
            predicates.Add(v => v.LicenceNumber.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        if (fields.Contains("Vehicle type"))
        {
            VehicleTypes value = AskForVehicleType();
            predicates.Add(v => v.VehicleType == value);
        }

/*         if (fields.Contains("Wheel count"))
        {
            int value = AskForWheelCount();
            predicates.Add(v => v.WheelCount == value);
        } */

        if (fields.Contains("Color"))
        {
            string value = AskForColor();
            predicates.Add(v => v.Color.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        if (fields.Contains("Max effect (HP)"))
        {
            int value = AskForEngineHP();
            predicates.Add(v => v.Engine.MaxPowerHP == value);
        }

        _menuPath.Pop();

        if (predicates.Count() == 0)
        {
            return null;
        }
        return v => predicates.All(p => p(v));
    }

    public void SearchResultWindow(Vehicle[] vehicles)
    {
        _menuPath.Push("Search result");
        RenderHeader();

        Table table = new();
        table.AddColumns("Licence number", "type", "Engine", "Wheels", "Color");

        foreach (var item in vehicles)
        {
            table.AddRow(
                item.LicenceNumber,
                item.VehicleType.ToString(),
                item.Engine.Description,
                // item.WheelCount.ToString(),
                item.Color
            );
        }

        _menuPath.Pop();

        AnsiConsole.Write(table);
    }
}
