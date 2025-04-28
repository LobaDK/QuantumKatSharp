using System.Collections;

namespace QuantumKat.Settings.Model;

public record class RootSettings
{
    public AppSettings App { get; set; }
    public DiscordSettings Discord { get; set; }
    public IDictionary Plugins { get; set; } = new Dictionary<object, object>();
}
