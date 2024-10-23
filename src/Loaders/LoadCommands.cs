using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Loaders;

public class LoadCommands
{
    public static void Load(DiscordClient client)
    {
        var slash = client.UseSlashCommands();
        slash.RegisterCommands<Commands.Status>();
        slash.RegisterCommands<Commands.Clear>();
        slash.RegisterCommands<Commands.Documentation>();
        slash.RegisterCommands<Commands.AddDevoir>();
        slash.RegisterCommands<Commands.DeleteDevoir>();
        slash.RegisterCommands<Commands.UpdateDevoir>();
    }
}
        