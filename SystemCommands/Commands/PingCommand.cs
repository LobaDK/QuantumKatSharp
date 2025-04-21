using Discord.WebSocket;
using SystemCommands.Interfaces;

namespace QuantumKat.Commands;

public class PingCommand(DiscordSocketClient client) : IPingCommand
{
    public string Ping(string message, bool measureLatency = false)
    {
        return DeterminePingResponse(message);
    }

    public string Ping(string message, DateTime messageCreatedAt, bool measureLatency = true)
    {
        string response = DeterminePingResponse(message);
        
        if (measureLatency)
        {
            int responseTime = GetLatency(messageCreatedAt)*2;
            int discordLatency = client.Latency;
            response = $"{response} Took {responseTime}ms. Connection to Discord servers: {discordLatency}ms";
        }

        return response;
    }

    private static string DeterminePingResponse(string message)
    {
        return message.ToLowerInvariant() switch
        {
            var m when m.Contains("ping") => "Pong!",
            var m when m.Contains("pong") => "Ping!",
            _ => "Unknown command. Use 'ping' or 'pong'.",
        };
    }

    private static int GetLatency(DateTime messageCreatedAt)
    {
        // Get local time
        DateTime localTime = DateTime.Now;

        //Subtract them to get the the time it took between them (assuming our clock is synced)
        TimeSpan latency = localTime - messageCreatedAt;

        return latency.Milliseconds;
    }
}
