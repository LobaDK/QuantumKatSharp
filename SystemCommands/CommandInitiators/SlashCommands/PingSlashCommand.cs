using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using QuantumKat.Commands;
using SystemCommands.Interfaces;

namespace SystemCommands.CommandInitiators.SlashCommands;

public class PingSlashCommand(DiscordSocketClient _client) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Ping the bot and receive a reply, with the option to also measure latency.")]
    public async Task Ping(bool measureLatency = false)
    {
        string response;
        if (measureLatency)
        {
            DateTime messageCreatedAt = Context.Interaction.CreatedAt.UtcDateTime;
            response = new PingCommand(_client).Ping("ping", messageCreatedAt, measureLatency);
        }
        else
        {
            response = new PingCommand(_client).Ping("ping", measureLatency);
        }

        await RespondAsync(response);
    }

    [SlashCommand("pong", "Ping the bot and receive a reply, with the option to also measure latency.")]
    public async Task Pong(bool measureLatency = false)
    {
        string response;
        if (measureLatency)
        {
            DateTime messageCreatedAt = Context.Interaction.CreatedAt.UtcDateTime;
            response = new PingCommand(_client).Ping("pong", messageCreatedAt, measureLatency);
        }
        else
        {
            response = new PingCommand(_client).Ping("pong", measureLatency);
        }

        await RespondAsync(response);
    }
}
