using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Linq;
using System.Threading.Tasks;

namespace discord_bot_csharp.Commands;

public class Clear : ApplicationCommandModule
{
    [SlashCommand("clear", "Commande pour supprimer des messages")]
    public async Task Command(InteractionContext ctx, [Option("nombre", "format: 10")] long count)
    {
        if (!await Functions.Permission.Get(ctx, 1280508888206282812))
        {
            return;
        }

        if (count <= 0)
        {
            var errorEmbed = new DiscordEmbedBuilder
            {
                Title = "Erreur",
                Description = "Le nombre de messages à supprimer doit être supérieur à 0",
                Color = DiscordColor.Red
            };

            await ctx.CreateResponseAsync(embed: errorEmbed.Build());
            await Task.Delay(3000);
            await ctx.DeleteResponseAsync();
            return;
        }

        var messages = await ctx.Channel.GetMessagesAsync((int)count);
        await ctx.Channel.DeleteMessagesAsync(messages);

        var successEmbed = new DiscordEmbedBuilder
        {
            Title = "Succès",
            Description = $"**{count}** messages ont été supprimés",
            Color = DiscordColor.Green
        };

        await ctx.CreateResponseAsync(embed: successEmbed.Build());
        await Task.Delay(3000);
        await ctx.DeleteResponseAsync();
    }
}