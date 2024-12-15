using Discord;
using Discord.WebSocket;
using QuantumKat.Utitlity;

namespace QuantumKat;

class Program
{
    private static DiscordSocketClient? _client;

    public static async Task Main(string[] args)
    {
        DiscordSocketConfig config = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);

        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;

        // TODO: Add dynamic way to control token type through external factors such as .vscode/launch.json and/or application config.
        await _client.LoginAsync(TokenType.Bot, await new TokenLoader("dev").LoadWith1Password());
        await _client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static Task ReadyAsync()
    {
        Console.WriteLine();
        return Task.CompletedTask;
    }

    private static Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}