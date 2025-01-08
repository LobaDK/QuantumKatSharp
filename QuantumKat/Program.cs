using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using QuantumKat.Utitlity;
using QuantumKat.Services;

namespace QuantumKat;

class Program
{
    private readonly static ServiceProvider _services = CreateServices();
    private readonly static string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
    public static async Task Main(string[] args)
    {
        if (!Directory.Exists(pluginPath))
        {
            Directory.CreateDirectory(pluginPath);
        }
        
        DiscordSocketClient client = _services.GetRequiredService<DiscordSocketClient>();

        // TODO: Look into keeping commands in external DLL's which can be loaded, reloaded and unloaded on the go

        client.Log += LogAsync;

        await _services.GetRequiredService<InteractionHandler>().InitializeAsync();

        // TODO: Add dynamic way to control token type through external factors. Thinking launch.json, appsettings/config and launch parameters
        await client.LoginAsync(TokenType.Bot, await new TokenLoader("dev").LoadWith1Password());
        await client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    private static ServiceProvider CreateServices()
    {
        InteractionServiceConfig interactionServiceConfig = new()
        {
            UseCompiledLambda = true
        };

        IServiceCollection collection = new ServiceCollection()
        .AddSingleton(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds
                | GatewayIntents.GuildMembers
                | GatewayIntents.GuildMessageReactions
                | GatewayIntents.GuildMessages
                | GatewayIntents.GuildMessageTyping
                | GatewayIntents.DirectMessageReactions
                | GatewayIntents.DirectMessages
                | GatewayIntents.DirectMessageTyping
                | GatewayIntents.MessageContent
        })
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), interactionServiceConfig))
        .AddSingleton<InteractionHandler>();

        return collection.BuildServiceProvider();
    }
}