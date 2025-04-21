using Discord.Commands;
using Discord.WebSocket;
using QuantumKat.Commands;

namespace SystemCommands.CommandInitiators.MessageCommands;

public class PingMessageCommand(DiscordSocketClient _client) : ModuleBase<SocketCommandContext>
{
    [Command("ping", false, "Ping the bot and receive a reply, with the option to also measure latency.")]
    [Alias("pong")]
    public async Task Ping(bool measureLatency = false)
    {
        string response;
        if (measureLatency)
        {
            DateTime messageCreatedAt = Context.Message.CreatedAt.UtcDateTime;
            response = new PingCommand(_client).Ping(Context.Message.Content[1..], messageCreatedAt, measureLatency);
        }
        else
        {
            response = new PingCommand(_client).Ping(Context.Message.Content[1..], measureLatency);
        }

        await ReplyAsync(response);
    }

}
