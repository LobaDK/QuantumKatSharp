namespace QuantumKat.Settings.Model;

public record class DiscordSettings
{
    public IEnumerable<string> GuildsToSyncTo { get; set; }
}
