using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using QuantumKat.Extensions;

namespace QuantumKat.Commands;

public class UserInteractions(DiscordSocketClient client) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly string[] QuantumLocationsPlural = ["dimensions", "universes", "realities", "timelines",];
    private readonly string[] QuantumLocationsSingular = ["dimension", "universe", "reality", "timeline",];
    private readonly string[] PetLoopChoices = ["pet", "pat", "petting", "patting"];

    [SlashCommand("pet", "Pets the specified user a random amount, with the option to specify how much.")]
    public async Task Pet(
        [Summary(description: "The user to pet")]
        IUser user,
        [Summary(description: "How many times they should be pet. Leave unselected to make it random. Must be between 1 and 100")] [MinValue(1)] [MaxValue(100)]
        int amount = 0)
    {
        Random random = new();
        if (amount == 0)
        {
            amount = random.Next(1, 100);
        }

        string verb = amount == 1 ? "time" : "times";

        string pets = $"pe{new('t', amount)}s";

        if (user.IsClient(client))
        {
            float x = random.NextSingle();

            // 45%
            if (x <= 0.45)
            {
                int QuantumSpan = random.Next(0, 100);
                string QuantumLocation = QuantumSpan == 1 ? QuantumLocationsSingular[random.Next(0, QuantumLocationsSingular.Length)] : QuantumLocationsPlural[random.Next(0, QuantumLocationsPlural.Length)];
                await RespondAsync($"*Quantum purrs across {QuantumSpan} {QuantumLocation}*");
            }
            // 45%
            else if (x <= 0.90)
            {
                int QuantumFrequency = random.Next(1, 100_000);
                await RespondAsync($"*Quantum vibrates at {QuantumFrequency}Hz*");
            }
            // 10%
            else
            {
                int PetLoopAmount = random.Next(4, 40);

                List<string> PetLoopSample = [];
                for (int i = 0; i < PetLoopAmount; i++)
                {
                    PetLoopSample.Add(PetLoopChoices[random.Next(PetLoopChoices.Length)]);
                }

                await RespondAsync($"Quantum Loop pet initiated trying to pet self! *{string.Join("", PetLoopSample)}*");
            }
        }
        else
        {
            await RespondAsync($"Superpositions {amount} {verb} around {user.Mention} and *{pets}*");
        }
    }

}
