using Discord.Interactions;

namespace QuantumKat.Commands;

public class Simple : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Ping the bot and receive a reply")]
    public async Task Ping()
    {
        await RespondAsync("Pong!");
    }
}
