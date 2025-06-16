using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using QuantumKat.Utitlity;
using QuantumKat.Services;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using QuantumKat.Settings.Model;
using QuantumKat.PluginSDK.Settings;
using QuantumKat.PluginSDK.Discord.Extensions;

namespace QuantumKat;

class Program
{
    public static async Task Main(string[] args)
    {
        SettingsManager settingsManager = new("config.ini");
        IConfiguration configuration = settingsManager.GetConfiguration<RootSettings>();

        RootSettings settings = new();
        ConfigurationBinder.Bind(configuration, settings);

        if (!Directory.Exists(settings.App.PluginPath))
        {
            Directory.CreateDirectory(settings.App.PluginPath);
        }

        DiscordSocketConfig socketConfig = new() {
            GatewayIntents = GatewayIntents.Guilds
            | GatewayIntents.GuildMembers
            | GatewayIntents.GuildMessageReactions
            | GatewayIntents.GuildMessages
            | GatewayIntents.GuildMessageTyping
            | GatewayIntents.DirectMessageReactions
            | GatewayIntents.DirectMessages
            | GatewayIntents.DirectMessageTyping
            | GatewayIntents.MessageContent
        };

        DiscordSocketClient client = new(socketConfig);

        client.Log += LogAsync;

        InteractionServiceConfig interactionServiceConfig = new() {
            UseCompiledLambda = true
        };

        InteractionService interactionService = new(client, interactionServiceConfig);

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(configuration);
        serviceCollection.AddSingleton(client);
        serviceCollection.AddSingleton(interactionService);
        serviceCollection.AddSingleton<InteractionHandler>();
        serviceCollection.AddSingleton<CommandService>();

        ServiceProvider services = serviceCollection.BuildServiceProvider();

        await services.GetRequiredService<InteractionHandler>().InitializeAsync();
        DiscordUserExtensions.Initialize(client);

        string bot_mode = Environment.GetEnvironmentVariable("TOKEN_TYPE") ?? "main";
        await client.LoginAsync(TokenType.Bot, await new TokenLoader(bot_mode).LoadWith1Password());
        await client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}