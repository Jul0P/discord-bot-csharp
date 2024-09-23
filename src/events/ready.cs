using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace discord_bot_csharp.Ready;

public static class Ready
{
    private static DiscordActivity activity { get; set; }

    public static async Task OnReady(DiscordClient client, ReadyEventArgs e)
    {
        await Task.Delay(1000);

        activity = new($"{client.Guilds.Values.FirstOrDefault().MemberCount} membres", ActivityType.Watching);

        await client.UpdateStatusAsync(activity);
    }
}
