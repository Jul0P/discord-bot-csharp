using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Commands;

public class Status : ApplicationCommandModule
{
    [SlashCommand("status", "Envoie un message de status")]
    public async Task Command(InteractionContext context)
    {
        await context.CreateResponseAsync("Le bot est bien actif !");
        await Task.Delay(3000);
        await context.DeleteResponseAsync();
    }
}