using Discord;
using Discord.WebSocket;

namespace QuantumKat.Extensions;

public static class DiscordUser
{
    /// <summary>
    /// Checks if the user is the client (bot) itself by comparing their IDs.
    /// </summary>
    /// <param name="user">The <c>IUser</c> object representing the user.</param>
    /// <param name="client">The <c>DiscordSocketClient</c> object representing the client.</param>
    /// <returns>Boolean indicating whether or not the user is the client.</returns>
    public static bool IsClient(this IUser user, DiscordSocketClient client)
    {
        return user.Id == client.CurrentUser.Id;
    }
}
