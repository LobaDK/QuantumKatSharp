using QuantumKat.Attributes;
using QuantumKat.Interfaces;
using QuantumKat.Settings;

namespace UserInteractionCommands.Settings.Model;

public record class PetCommandSettings : IPluginSettings
{
    [SettingCallback(nameof(PromptForQuantumLocationsPlural))]
    public IEnumerable<string> QuantumLocationsPlural { get; set; }

    [SettingCallback(nameof(PromptForQuantumLocationsSingular))]
    public IEnumerable<string> QuantumLocationsSingular { get; set; }

    [SettingCallback(nameof(PromptForPetLoopChoices))]
    public IEnumerable<string> PetLoopChoices { get; set; }

    public static List<string> PromptForQuantumLocationsPlural()
    {
        List<string> locations = [];
        string? input = null;

        Console.WriteLine("Do you want to use the default plural quantum locations? (Y/n) ['dimensions', 'universes', 'realities', 'timelines']");
        ConsoleKeyInfo response = Console.ReadKey();
        if (response.Key == ConsoleKey.Y)
        {
            return ["dimensions", "universes", "realities", "timelines"];
        }

        while (input != "")
        {
            Console.WriteLine("Please provide a location for the pet to possibly originate from in plural. Press Enter without typing anything to stop adding locations: ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                locations.Add(input);
            }
        }

        return locations;
    }

    public static List<string> PromptForQuantumLocationsSingular()
    {
        List<string> locations = [];
        string? input = null;

        Console.WriteLine("Do you want to use the default singular quantum locations? (Y/n) ['dimension', 'universe', 'reality', 'timeline']");
        ConsoleKeyInfo response = Console.ReadKey();
        if (response.Key == ConsoleKey.Y)
        {
            return ["dimension", "universe", "reality", "timeline"];
        }

        while (input != "")
        {
            Console.WriteLine("Please provide a location for the pet to possibly originate from in singular. Press Enter without typing anything to stop adding locations: ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                locations.Add(input);
            }
        }

        return locations;
    }

    public static List<string> PromptForPetLoopChoices()
    {
        List<string> locations = [];
        string? input = null;

        Console.WriteLine("Do you want to use the default pet loop choices? (Y/n) ['pet', 'pat', 'petting', 'patting']");
        ConsoleKeyInfo response = Console.ReadKey();
        if (response.Key == ConsoleKey.Y)
        {
            return ["pet", "pat", "petting", "patting"];
        }

        while (input != "")
        {
            Console.WriteLine("Please provide the possible pet choices when the bot enters a petloop. Press Enter without typing anything to stop adding locations: ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                locations.Add(input);
            }
        }

        return locations;
    }

    public object GetDefaultSettings()
    {
        return SettingsManager.InitializeSettings<PetCommandSettings>();
    }
}
