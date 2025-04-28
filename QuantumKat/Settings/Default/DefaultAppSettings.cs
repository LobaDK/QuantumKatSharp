using QuantumKat.Attributes;

namespace QuantumKat.Settings.Default;

public record class DefaultAppSettings
{
    [SettingCallback(nameof(PromptForPluginPath))]
    public string PluginPath { get; set; }

    public static string PromptForPluginPath()
    {
        string? path = null;
        
        while (path is null)
        {
            Console.WriteLine("Please provide the path to the plugins folder. If no value is given, the default './plugins' will be used: ");
            path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "plugins");
            }
        }
        return path;
    }
}
