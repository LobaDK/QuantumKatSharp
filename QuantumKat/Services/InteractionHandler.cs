using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuantumKat.Settings.Model;
using QuantumKat.PluginSDK.Discord;
using QuantumKat.PluginSDK.Settings;
using QuantumKat.PluginSDK;
using QuantumKat.PluginSDK.Discord.Extensions;

namespace QuantumKat.Services;

public class InteractionHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly CommandService _commandService;
    private readonly IConfiguration _configuration;
    private readonly RootSettings _settings = new();

    public InteractionHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _client = serviceProvider.GetRequiredService<DiscordSocketClient>();
        _interactionService = serviceProvider.GetRequiredService<InteractionService>();
        _commandService = serviceProvider.GetRequiredService<CommandService>();
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        ConfigurationBinder.Bind(_configuration, _settings);
    }

    private readonly List<IOnMessageEvent> _messageHandlerPlugins = [];

    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _interactionService.Log += LogAsync;

        SettingsManager settingsManager = new("config.ini");

        // TODO: Improve external DLL loading. Require use of custom interface to ensure correct structure?
        // TODO: Introduce commands or other form of control to load, unload, and reload DLLs.
        foreach (string dll in Directory.GetFiles(_settings.App.PluginPath, "*.dll"))
        {
            // TODO: Make this a logged message
            Console.WriteLine($"Loading {new FileInfo(dll).Name}");
            await _interactionService.AddModulesAsync(Assembly.LoadFrom(dll), _serviceProvider);
            await _commandService.AddModulesAsync(Assembly.LoadFrom(dll), _serviceProvider);

            Assembly assembly = Assembly.LoadFrom(dll);
            foreach(Type type in assembly.GetTypes())
            {
                if (typeof(IOnMessageEvent).IsAssignableFrom(type))
                {
                    IOnMessageEvent plugin = (IOnMessageEvent)Activator.CreateInstance(type, _serviceProvider)!;
                    _messageHandlerPlugins.Add(plugin);
                }
                else if (typeof(ISetting).IsAssignableFrom(type))
                {
                    ISetting pluginSettings = (ISetting)Activator.CreateInstance(type)!;

                    string assemblyName = pluginSettings.SectionName;
                    if (!_settings.Plugins.Contains(assemblyName))
                    {
                        pluginSettings = (ISetting)pluginSettings.GetDefaultSettings();
                        _settings.Plugins.Add(assemblyName, pluginSettings);
                        settingsManager.Save(_settings);
                    }
                }
            }

        }

        _client.InteractionCreated += HandleInteraction;
        _client.MessageReceived += HandleMessageAsync;
    }
    
    private Task LogAsync(LogMessage logMessage)
    {
        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }
    
    private async Task ReadyAsync()
    {
        foreach (string guildToSyncTo in _settings.Discord.GuildsToSyncTo)
        {
            ulong guild_id = ulong.Parse(guildToSyncTo);
            if (guildToSyncTo.Length == 18)
            {
                SocketGuild guild = _client.GetGuild(guild_id);
                Console.WriteLine($"Registering commands to guild {guild.Name} ({guild.Id})");
                await _interactionService.RegisterCommandsToGuildAsync(guild_id);
            }
            else if (guildToSyncTo.Length == 19)
            {
                IChannel channel = await _client.GetChannelAsync(guild_id);
                if (channel is SocketTextChannel textChannel)
                {
                    Console.WriteLine($"Registering commands to channel {textChannel.Name} ({textChannel.Id})");
                    await _interactionService.RegisterCommandsToGuildAsync(textChannel.Guild.Id);
                }
                else
                {
                    Console.WriteLine($"Unable to register commands to channel {guild_id}. Channel is not a text channel.");
                }
            }
        }
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

    private async Task HandleMessageAsync(SocketMessage socketMessage)
    {
        if (socketMessage is not SocketUserMessage message)
        {
            return;
        }

        // Ignore messages from the bot itself
        if (message.Author.IsClient())
        {
            return;
        }

        int argPos = 0;

        // Notify any plugins that implemented the IOnMessageEvent interface
        foreach (IOnMessageEvent plugin in _messageHandlerPlugins)
        {
            await plugin.HandleMessageAsync(message);
        }

        // Ignore messages that don't start with the command prefix or are from bots
        if (!message.HasCharPrefix('?', ref argPos) || message.Author.IsBot)
        {
            return;
        }

        // Create a command context for the message
        SocketCommandContext commandContext = new(_client, message);

        // Attempt to execute a text-based command
        await _commandService.ExecuteAsync(commandContext, argPos, _serviceProvider);
    }
}
