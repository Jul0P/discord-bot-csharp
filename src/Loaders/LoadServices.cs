using DSharpPlus;

namespace discord_bot_csharp.Loaders;

public class LoadServices
{
    public static async Task Load(DiscordClient client)
    {
        client.Ready += Services.RichPresence.Ready;
        await Services.Devoir.Init(client);
    }
}
        