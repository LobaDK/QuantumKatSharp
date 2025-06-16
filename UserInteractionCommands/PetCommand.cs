using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuantumKat.PluginSDK.Discord.Extensions;
using UserInteractionCommands.Settings.Model;


namespace UserInteractionCommands;

public class UserInteractions : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly PetCommandSettings _settings = new();

    public UserInteractions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        ConfigurationBinder.Bind(_configuration.GetSection($"plugins:{_settings.SectionName}"), _settings);
    }

    [SlashCommand("pet", "Pets the specified user a random amount, with the option to specify how much.")]
    public async Task Pet(
        [Summary(description: "The user to pet")]
        IUser user,
        [Summary(description: "How many times they should be pet. Leave unselected to make it random. Must be between 1 and 100")]
        [MinValue(1)] [MaxValue(100)]
        int amount = 0)
    {
        Random random = new();

        if (amount == 0)
        {
            amount = random.Next(1, 100);
        }

        if (user.IsClient())
        {
            float x = random.NextSingle();

            // TODO: Store and retrieve the probability of each option in global config for "tuneable"-like modifications
            // 45%
            if (x <= 0.45)
            {
                string QuantumLocation = amount == 1
                    ? _settings.QuantumLocationsSingular.ElementAt(random.Next(0, _settings.QuantumLocationsSingular.Count()))
                    : _settings.QuantumLocationsPlural.ToList()[random.Next(0, _settings.QuantumLocationsPlural.Count())];
                await RespondAsync($"*Quantum purrs across {amount} {QuantumLocation}*");
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
                    PetLoopSample.Add(_settings.PetLoopChoices.ElementAt(random.Next(_settings.PetLoopChoices.Count())));
                }

                await RespondAsync($"Quantum Loop pet initiated trying to pet self! *{string.Join("", PetLoopSample)}*");
            }
        }
        else
        {
            string verb = amount == 1 ? "time" : "times";
            string pets = $"pe{new('t', amount)}s";
            await RespondAsync($"Superpositions {amount} {verb} around {user.Mention} and *{pets}*");
        }
    }

}
