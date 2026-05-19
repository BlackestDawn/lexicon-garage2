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
    private readonly IStatusProvider _usageStatus;
    private readonly Stack _menuPath = new(5);
    private readonly Dictionary<VehicleTypes, Color> _typesColor = new()
    {
        { VehicleTypes.Car, Color.Magenta },
        { VehicleTypes.Bus, Color.LightGreen },
        { VehicleTypes.Motorcycle, Color.Cyan },
        { VehicleTypes.Boat, Color.DarkBlue },
        { VehicleTypes.Airplane, Color.LightPink4 }
    };

    public ConsoleUI(IStatusProvider getStatus)
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
            menuPathSB.PrependLine($"{new string(' ', 2 * depthCounter)}{item}");
            depthCounter--;
        }

        Panel menuPathPanel = new Panel(menuPathSB.ToString())
            .Header("  Menu path")
            .Expand()
            .NoBorder()
            .Padding(0, 2);

        var currentStatus = _usageStatus.GetStatus();

        Panel usagePanel = new Panel(
            new BreakdownChart()
                .Width(20)
                .AddItem("Used:", currentStatus.Used, Color.Red3)
                .AddItem("Free:", currentStatus.Capacity - currentStatus.Used, Color.LightYellow3)
            )
            .Header("Space usage")
            .NoBorder()
            .Padding(2, 1);

        BreakdownChartItem[] typesBreakdown = new BreakdownChartItem[Enum.GetNames<VehicleTypes>().Length];

        foreach (var item in currentStatus.TypesCount)
        {
            typesBreakdown[(int)item.Key] = new BreakdownChartItem($"{item.Key}", currentStatus.TypesCount[item.Key], _typesColor[item.Key]);
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
        AnsiConsole.MarkupLine($"[gray]\n{message}[/]");
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

    public Vehicle VehicleListSelectionWindow(IEnumerable<Vehicle> vehicles)
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

    public string RemoveVehicleWindow(IEnumerable<string> licenceNumbers)
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

    private string AskForPartialLicenceNumber()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter licence number:")
                .Validate(input => input.Length >= 1, "Must be at least 1 character")
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

        _menuPath.Pop();
        _menuPath.Pop();
        PauseDisplay();
    }

    public void Message(string content, MessageTypes type = MessageTypes.Standard)
    {
        string color = type switch
        {
            MessageTypes.Info => "gray",
            MessageTypes.Success => "green",
            MessageTypes.Warning => "yellow",
            MessageTypes.Error => "red",
            _ => "white",
        };
        AnsiConsole.MarkupLine($"[{color}]{content}[/]");
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
                    "Max effect (HP)",
                    "Max speed",
                    "Car type",
                    "Engine count",
                    "Passenger capacity"
                ])
            );

        var predicates = new List<Func<Vehicle, bool>>();

        if (fields.Contains("Licence number"))
        {
            string value = AskForPartialLicenceNumber();
            predicates.Add(v => v.LicenceNumber.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        if (fields.Contains("Vehicle type"))
        {
            VehicleTypes value = AskForVehicleType();
            predicates.Add(v => v.VehicleType == value);
        }

        if (fields.Contains("Wheel count"))
        {
            int value = AskForWheelCount();
            predicates.Add(v => v is WheeledVehicle wv && wv.WheelCount == value);
        }

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

        if (fields.Contains("Max speed"))
        {
            int value = AskForMaxSpeed();
            predicates.Add(v =>
                (v is Car car && car.MaxSpeed == value) ||
                (v is Motorcycle motorcycle && motorcycle.MaxSpeed == value)
            );
        }

        if (fields.Contains("Car type"))
        {
            CarTypes value = AskForCarType();
            predicates.Add(v => v is Car car && car.CarType == value);
        }

        if (fields.Contains("Engine count"))
        {
            int value = AskForEngineCount();
            predicates.Add(v =>
                (v is Airplane airplane && airplane.EngineCount == value) ||
                (v is Boat boat && boat.EngineCount == value)
            );
        }

        if (fields.Contains("Passenger capacity"))
        {
            int value = AskForPassengerCount();
            predicates.Add(v => v is Bus bus && bus.PassengerCapacity == value);
        }

        _menuPath.Pop();

        if (predicates.Count() == 0)
        {
            return null;
        }
        return v => predicates.All(p => p(v));
    }

    public void SearchResultWindow(IEnumerable<Vehicle> vehicles)
    {
        _menuPath.Push("Search result");
        RenderHeader();

        var list = vehicles.ToList();

        // Detekt and build dynamic columns
        bool hasWheels = list.Any(v => v is WheeledVehicle);
        bool hasCarType = list.Any(v => v is Car);
        bool hasMaxSpeed = list.Any(v => v is Car || v is Motorcycle);
        bool hasEngineCount = list.Any(v => v is Airplane || v is Boat);
        bool hasPassengers = list.Any(v => v is Bus);

        var columns = new List<string> {"Licence number", "Vehicle type", "Engine", "Color"};
        if (hasWheels) columns.Add("Wheels");
        if (hasCarType) columns.Add("Car type");
        if (hasMaxSpeed) columns.Add("Max speed");
        if (hasEngineCount) columns.Add("Engine count");
        if (hasPassengers) columns.Add("Passenger capacity");

        // Build table
        Table table = new();
        table.AddColumns(columns.ToArray());

        foreach (var item in list)
        {
            var row = new List<string>
            {
                item.LicenceNumber,
                item.VehicleType.ToString(),
                item.Engine.Description,
                item.Color
            };

            if (hasWheels) row.Add(item is WheeledVehicle v2 ? v2.WheelCount.ToString() : "-");
            if (hasCarType) row.Add(item is Car v2 ? v2.CarType.ToString() : "-");
            if (hasMaxSpeed) row.Add(item is Car v2 ? v2.WheelCount.ToString() :
                                    item is Motorcycle v3 ? v3.MaxSpeed.ToString() : "-");
            if (hasEngineCount) row.Add(item is Airplane v2 ? v2.EngineCount.ToString() :
                                    item is Boat v3 ? v3.EngineCount.ToString() : "-");
            if (hasPassengers) row.Add(item is Bus v2 ? v2.PassengerCapacity.ToString() : "-");

            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);

        _menuPath.Pop();
        PauseDisplay();
    }
}
