using Discord.Interactions;

namespace QuantumKat.Commands;

public class PingCommand : InteractionModuleBase<SocketInteractionContext>
{
    // TODO: Add parameter to also get latency
    [SlashCommand("ping", "Ping the bot and receive a reply")]
    public async Task Ping()
    {
        await RespondAsync("Pong!");
    }
}
