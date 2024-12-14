using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace QuantumKat;

class Program
{
    private static DiscordSocketClient _client;

    public static async Task Main(string[] args)
    {
        DiscordSocketConfig config = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);

        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
    }

    private static Task ReadyAsync()
    {
        System.Console.WriteLine();
        return Task.CompletedTask;
    }

    private static Task LogAsync(LogMessage message)
    {
        System.Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}