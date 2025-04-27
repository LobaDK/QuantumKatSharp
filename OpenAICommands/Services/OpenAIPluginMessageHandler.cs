using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using QuantumKat.Interfaces;
using System.Text.RegularExpressions;

namespace OpenAICommands.Services;

public class OpenAIPluginMessageHandler : IMessageHandlerPlugin
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _client;
    
    public OpenAIPluginMessageHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
    }

    public async Task HandleMessageAsync(SocketUserMessage message, int argPos)
    {
        string pattern = $@"^hey[, ]+(quantumkat|<@{_client.CurrentUser.Id}>)";
        if (Regex.IsMatch(message.Content, pattern, RegexOptions.IgnoreCase))
        {
            await message.ReplyAsync("Hello!");
        }
    }
}