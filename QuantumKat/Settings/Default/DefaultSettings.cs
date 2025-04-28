namespace QuantumKat.Settings.Default;

public record class DefaultSettings
{
    public DefaultAppSettings App { get; set; }
    public DefaultDiscordSettings Discord { get; set; }
}
