using Discord.WebSocket;

namespace SystemCommands.Interfaces;

public interface IPingCommand
{
    public string Ping(string message, bool measureLatency = false);
    public string Ping(string message, DateTime messageCreatedAt, bool measureLatency = true);
}
