using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Commands;

public class Clear : ApplicationCommandModule
{
    [SlashCommand("clear", "Commande pour supprimer des messages")]
    public async Task Command(InteractionContext context, [Option("nombre", "format: 10")] long count)
    {
        if (!await Functions.Permission.Get(context, 1280508888206282812))
        {
            return;
        }

        if (count <= 0)
        {
            DiscordEmbedBuilder errorEmbed = new DiscordEmbedBuilder
            {
                Title = "Erreur",
                Description = "Le nombre de messages à supprimer doit être supérieur à 0",
                Color = DiscordColor.Red
            };

            await context.CreateResponseAsync(embed: errorEmbed.Build());
            await Task.Delay(3000);
            await context.DeleteResponseAsync();
            return;
        }

        IReadOnlyList<DiscordMessage> messages = await context.Channel.GetMessagesAsync((int)count);
        await context.Channel.DeleteMessagesAsync(messages);

        DiscordEmbedBuilder successEmbed = new DiscordEmbedBuilder
        {
            Title = "Succès",
            Description = $"**{count}** messages ont été supprimés",
            Color = DiscordColor.Green
        };

        await context.CreateResponseAsync(embed: successEmbed.Build());
        await Task.Delay(3000);
        await context.DeleteResponseAsync();
    }
}