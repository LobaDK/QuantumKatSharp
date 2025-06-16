using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using QuantumKat.PluginSDK.Discord;
using QuantumKat.PluginSDK.Discord.Extensions;

namespace OpenAICommands.Services;

public class OpenAIPluginMessageHandler : IOnMessageEvent
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _client;
    
    public OpenAIPluginMessageHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
    }

    public async Task HandleMessageAsync(SocketMessage message)
    {
        if (message.IsFromBot())
        {
            return;
        }

        if (message.IsUserMessage(out SocketUserMessage userMessage))
        {
            // Catches messages that start with "hey quantumkat" or "hey @quantumkat"
            // including comma and space variations.
            string pattern = $@"^hey[, ]+(quantumkat|<@{_client.CurrentUser.Id}>)";
            if (Regex.IsMatch(message.Content, pattern, RegexOptions.IgnoreCase))
            {
                await userMessage.ReplyAsync("Hello!");
            }
        }
    }
}