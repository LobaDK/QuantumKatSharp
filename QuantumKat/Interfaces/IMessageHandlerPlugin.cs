using Discord.WebSocket;

namespace QuantumKat.Interfaces;

public interface IMessageHandlerPlugin
{
    Task HandleMessageAsync(SocketUserMessage message, int argPos);
}
