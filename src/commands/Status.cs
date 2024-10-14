using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Commands;

public class Status : ApplicationCommandModule
{
    [SlashCommand("status", "Envoie un message de status")]
    public async Task StatusCommand(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync("Le bot est bien actif !");
        await Task.Delay(3000);
        await ctx.DeleteResponseAsync();
    }
}