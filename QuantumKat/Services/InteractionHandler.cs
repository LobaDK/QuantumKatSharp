using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace QuantumKat.Services;

public class InteractionHandler(DiscordSocketClient _client, InteractionService _interactionService, IServiceProvider _serviceProvider)
{
    // TODO: This should be stored and used in the global config instead
    private readonly static string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _interactionService.Log += LogAsync;

        // TODO: Improve external DLL loading. Require use of custom interface to ensure correct structure?
        // TODO: Introduce commands or other form of control to load, unload, and reload DLLs.
        foreach (string dll in Directory.GetFiles(pluginPath, "*.dll"))
        {
            // TODO: Make this a logged message
            Console.WriteLine($"Loading {new FileInfo(dll).Name}");
            await _interactionService.AddModulesAsync(Assembly.LoadFrom(dll), _serviceProvider);
        }

        _client.InteractionCreated += HandleInteraction;
    }
    private Task LogAsync(LogMessage logMessage)
    {
        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }
    private async Task ReadyAsync()
    {
        // TODO: Use the global config
        await _interactionService.RegisterCommandsToGuildAsync(665680289510588447);
    }
    private async Task HandleInteraction(SocketInteraction socketInteraction)
    {
        try
        {
            SocketInteractionContext context = new(_client, socketInteraction);
            await _interactionService.ExecuteCommandAsync(context, _serviceProvider);
        }
        catch
        {
            if (socketInteraction.Type is InteractionType.ApplicationCommand)
                await socketInteraction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }
}
