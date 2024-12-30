using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace QuantumKat.Services;

public class InteractionHandler(DiscordSocketClient _client, InteractionService _interactionService, IServiceProvider _serviceProvider)
{
    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _interactionService.Log += LogAsync;

        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

        _client.InteractionCreated += HandleInteraction;
    }
    private Task LogAsync(LogMessage logMessage)
    {
        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }
    private async Task ReadyAsync()
    {
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
