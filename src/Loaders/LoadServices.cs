using DSharpPlus;

namespace discord_bot_csharp.Loaders;

public static class LoadServices
{
    public static async Task Load(DiscordClient client)
    {
        client.Ready += Services.RichPresence.Ready;
        await Services.DevoirService.Init(client);
    }
}
        