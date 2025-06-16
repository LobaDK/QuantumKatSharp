using QuantumKat.PluginSDK.Attributes;

namespace QuantumKat.Settings.Model;

public record class DiscordSettings
{
    [SettingCallback(nameof(PromptForGuildsToSyncTo))]
    public IEnumerable<string> GuildsToSyncTo { get; set; } = [];

    public static IEnumerable<string> PromptForGuildsToSyncTo()
    {
        var guilds = new List<string>();
        string? input = null;

        while (input != "")
        {
            Console.WriteLine("Please provide the ID of a guild to sync to. Press Enter without typing anything to stop adding guilds: ");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                guilds.Add(input);
            }
        }

        return guilds;
    }
}
