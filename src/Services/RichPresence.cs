using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace discord_bot_csharp.Services;

public static class RichPresence
{
    private static DiscordActivity activity { get; set; }

    public static async Task Ready(DiscordClient client, ReadyEventArgs e)
    {
        await Task.Delay(1000);

        activity = new($"{client.Guilds.Values.FirstOrDefault().MemberCount} membres", ActivityType.Watching);

        await client.UpdateStatusAsync(activity);
    }
}
