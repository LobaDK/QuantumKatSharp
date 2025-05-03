namespace QuantumKat.Interfaces;

public interface IPluginSettings
{
    public string EntryKey { get; }

    object GetDefaultSettings();
}
