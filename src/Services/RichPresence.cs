using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace discord_bot_csharp.Services;

public class RichPresence
{
    public static async Task Ready(DiscordClient client, ReadyEventArgs e)
    {
        await Task.Delay(1000);

        DiscordActivity activity = new DiscordActivity($"{client.Guilds.Values.FirstOrDefault().MemberCount} membres", ActivityType.Watching);

        await client.UpdateStatusAsync(activity);
    }
}
